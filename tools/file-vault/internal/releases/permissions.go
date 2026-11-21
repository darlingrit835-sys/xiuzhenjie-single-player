package releases

import (
	"os"
	"syscall"
)

func setReleasePermissions(file *os.File, directory os.FileInfo) error {
	group := directory.Sys().(*syscall.Stat_t).Gid
	if err := file.Chown(-1, int(group)); err != nil {
		return err
	}
	return file.Chmod(0640)
}
