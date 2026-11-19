import com.google.protobuf.DescriptorProtos.FileDescriptorSet;
import com.google.protobuf.Descriptors.FileDescriptor;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.HashSet;
import java.util.Set;
import java.util.TreeSet;
import java.util.regex.Matcher;
import java.util.regex.Pattern;
import java.util.stream.Collectors;
import xzj.Xzj;

class ExportProtocol {
    private static void add(FileDescriptor source, Set<String> seen, FileDescriptorSet.Builder output) {
        if (!seen.add(source.getFullName())) return;
        for (FileDescriptor dependency : source.getDependencies()) add(dependency, seen, output);
        output.addFile(source.toProto());
    }

    public static void main(String[] args) throws Exception {
        Path dest = Path.of(args[0]);
        FileDescriptorSet.Builder descriptors = FileDescriptorSet.newBuilder();
        add(Xzj.getDescriptor(), new HashSet<>(), descriptors);
        Files.write(dest.resolve("xzj.pb"), descriptors.build().toByteArray());

        Pattern registration = Pattern.compile("GameServer\\.register\\((\\d+),");
        Matcher matches = registration.matcher(Files.readString(Path.of(args[1])));
        Set<Integer> commands = new TreeSet<>();
        while (matches.find()) commands.add(Integer.parseInt(matches.group(1)));
        Files.write(dest.resolve("java-commands.txt"), commands.stream().map(Object::toString).collect(Collectors.toList()));
    }
}
