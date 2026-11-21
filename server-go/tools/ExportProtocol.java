import com.google.protobuf.DescriptorProtos.DescriptorProto;
import com.google.protobuf.DescriptorProtos.FieldDescriptorProto;
import com.google.protobuf.DescriptorProtos.FieldDescriptorProto.Type;
import com.google.protobuf.DescriptorProtos.FileDescriptorProto;
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
        // The Java fallback stays wire-compatible with version zero. Its descriptor
        // does not contain the optional Go/Unity exploration fields.
        FileDescriptorProto.Builder file = source.toProto().toBuilder();
        for (DescriptorProto.Builder message : file.getMessageTypeBuilderList()) {
            switch (message.getName()) {
                case "EnterEquipmentResp_1009005":
                    for (DescriptorProto.Builder child : message.getNestedTypeBuilderList()) {
                        if (!child.getName().equals("EquipmentInfoResp_1009006")) continue;
                        addField(child, "BlankId", 8, Type.TYPE_STRING);
                        addField(child, "TemplateId", 9, Type.TYPE_STRING);
                        addField(child, "IsLock", 10, Type.TYPE_BOOL);
                    }
                    break;
                case "SpaceGoodsResp_1004002":
                    for (DescriptorProto.Builder child : message.getNestedTypeBuilderList()) {
                        if (!child.getName().equals("GoodsInfoResp_1004003")) continue;
                        addField(child, "BlankId", 15, Type.TYPE_STRING);
                        addField(child, "OfferPending", 16, Type.TYPE_BOOL);
                    }
                    break;
                case "EnterBattleHomeResp_1005038":
                    addField(message, "CurrentVigor", 4, Type.TYPE_INT32);
                    for (DescriptorProto.Builder child : message.getNestedTypeBuilderList()) {
                        if (!child.getName().equals("SkillResp_1005039")) continue;
                        addField(child, "ForceEffort", 10, Type.TYPE_INT32);
                        addField(child, "PracticeAffix", 11, Type.TYPE_INT32);
                        addField(child, "PendingAffix", 12, Type.TYPE_INT32);
                        addField(child, "PracticeRevision", 13, Type.TYPE_INT32);
                        addField(child, "BaseCastMS", 14, Type.TYPE_INT32);
                    }
                    break;
                case "PracticeSkillReq_1005040":
                    addField(message, "PracticeAction", 3, Type.TYPE_INT32);
                    addField(message, "ExpectedRevision", 4, Type.TYPE_INT32);
                    break;
                case "PracticeSkillResp_1005040":
                    addField(message, "ForceEffort", 9, Type.TYPE_INT32);
                    addField(message, "PracticeAffix", 10, Type.TYPE_INT32);
                    addField(message, "PendingAffix", 11, Type.TYPE_INT32);
                    addField(message, "PracticeRevision", 12, Type.TYPE_INT32);
                    break;
                case "RoleJoinDungeonResp_1010002":
                    for (DescriptorProto.Builder child : message.getNestedTypeBuilderList()) {
                        if (!child.getName().equals("BattleSkillResp_1010003")) continue;
                        addField(child, "PracticeAffix", 26, Type.TYPE_INT32);
                        addField(child, "ForgeResonancePercent", 27, Type.TYPE_INT32);
                    }
                    break;
                case "BattleRoleEnterReq_8001001":
                    addField(message, "WorldVersion", 3, Type.TYPE_INT32);
                    break;
                case "BattleRoleEnterResp_8001001":
                    addField(message, "WorldTiles", 16, Type.TYPE_BYTES);
                    addField(message, "RegionId", 17, Type.TYPE_STRING);
                    addField(message, "Explored", 18, Type.TYPE_BYTES);
                    addField(message, "Landmarks", 19, Type.TYPE_BYTES);
                    for (DescriptorProto.Builder nested : message.getNestedTypeBuilderList()) {
                        if (nested.getName().equals("DropGoodsInfoResp_8001013")) {
                            addField(nested, "Count", 11, Type.TYPE_INT32);
                        }
                    }
                    break;
            }
        }
        output.addFile(file);
    }

    private static void addField(DescriptorProto.Builder message, String name, int number, Type type) {
        for (FieldDescriptorProto field : message.getFieldList()) {
            if (field.getName().equals(name) && field.getNumber() == number && field.getType() == type) return;
            if (field.getNumber() == number || field.getName().equals(name)) {
                throw new IllegalArgumentException(message.getName() + "." + name + " conflicts with Java descriptor");
            }
        }
        message.addFieldBuilder().setName(name).setNumber(number).setType(type)
                .setLabel(FieldDescriptorProto.Label.LABEL_OPTIONAL);
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
