package releases

import (
	"archive/zip"
	"bytes"
	"encoding/json"
	"errors"
	"fmt"
	"io"
	"io/fs"
	"os"
	"path/filepath"
	"strconv"

	"github.com/shogo82148/androidbinary"
)

const packageName = "com.lmgame.xiuxianjie.emailtrial"
const apkName = "client.apk"

type Release struct {
	Name    string `json:"name"`
	Version int32  `json:"versionCode"`
	Size    int64  `json:"size"`
}

func Read(dir string, version int32) (Release, error) {
	var release Release
	metadata, err := os.ReadFile(filepath.Join(dir, strconv.Itoa(int(version)), "release.json"))
	if err != nil {
		return release, err
	}
	err = json.Unmarshal(metadata, &release)
	if err == nil && release.Version != version {
		err = errors.New("release version does not match directory")
	}
	return release, err
}

func Latest(dir string) (Release, error) {
	entries, err := os.ReadDir(dir)
	if err != nil {
		return Release{}, err
	}
	var latest int32
	for _, entry := range entries {
		version, err := strconv.ParseInt(entry.Name(), 10, 32)
		if entry.IsDir() && err == nil && int32(version) > latest {
			latest = int32(version)
		}
	}
	if latest == 0 {
		return Release{}, fs.ErrNotExist
	}
	return Read(dir, latest)
}

func (r Release) Path(dir string) string {
	return filepath.Join(dir, strconv.Itoa(int(r.Version)), apkName)
}

// Publish links a complete APK into a private staging directory before making it visible.
func Publish(dir string, file *os.File, source, name string, size int64) (int32, error) {
	version, err := apkVersion(file, size)
	if err != nil || version <= 0 {
		return 0, err
	}
	latest, err := Latest(dir)
	if err != nil && !errors.Is(err, fs.ErrNotExist) {
		return 0, err
	}
	if version <= latest.Version {
		return 0, nil
	}

	staging, err := os.MkdirTemp(dir, ".staging-")
	if err != nil {
		return 0, err
	}
	defer os.RemoveAll(staging)
	// The vault runs with the download service's group. DynamicUser forbids setting setgid.
	if err := os.Chmod(staging, 0750); err != nil {
		return 0, err
	}
	folder, err := os.Stat(staging)
	if err != nil {
		return 0, err
	}
	if err := setReleasePermissions(file, folder); err != nil {
		return 0, err
	}
	if err := os.Link(source, filepath.Join(staging, apkName)); err != nil {
		return 0, err
	}
	metadata, err := json.Marshal(Release{Name: name, Version: version, Size: size})
	if err != nil {
		return 0, err
	}
	if err := os.WriteFile(filepath.Join(staging, "release.json"), metadata, 0640); err != nil {
		return 0, err
	}
	if err := os.Rename(staging, filepath.Join(dir, strconv.Itoa(int(version)))); err != nil {
		return 0, err
	}
	return version, nil
}

func apkVersion(reader io.ReaderAt, size int64) (int32, error) {
	archive, err := zip.NewReader(reader, size)
	if err != nil {
		return 0, err
	}
	for _, entry := range archive.File {
		if entry.Name != "AndroidManifest.xml" {
			continue
		}
		const maxManifest = 1 << 20
		if entry.UncompressedSize64 > maxManifest {
			return 0, errors.New("manifest too large")
		}
		stream, err := entry.Open()
		if err != nil {
			return 0, err
		}
		data, err := io.ReadAll(io.LimitReader(stream, maxManifest+1))
		if err := errors.Join(err, stream.Close()); err != nil {
			return 0, err
		}
		if len(data) > maxManifest {
			return 0, errors.New("manifest too large")
		}
		document, err := androidbinary.NewXMLFile(bytes.NewReader(data))
		if err != nil {
			return 0, err
		}
		var manifest struct {
			Package string              `xml:"package,attr"`
			Version androidbinary.Int32 `xml:"http://schemas.android.com/apk/res/android versionCode,attr"`
		}
		if err := document.Decode(&manifest, nil, nil); err != nil {
			return 0, err
		}
		if manifest.Package != packageName {
			return 0, nil
		}
		version, err := manifest.Version.Int32()
		if err != nil || version <= 0 {
			return 0, fmt.Errorf("invalid APK version code %d", version)
		}
		return version, nil
	}
	return 0, errors.New("APK missing AndroidManifest.xml")
}
