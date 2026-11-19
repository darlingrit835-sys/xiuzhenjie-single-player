package server.account;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.sun.net.httpserver.HttpExchange;
import com.sun.net.httpserver.HttpHandler;
import com.sun.net.httpserver.HttpsConfigurator;
import com.sun.net.httpserver.HttpsServer;
import server.common.Config;
import server.common.JsonStore;
import server.common.ServerConfig;

import javax.net.ssl.KeyManagerFactory;
import javax.net.ssl.SSLContext;
import java.io.*;
import java.net.InetSocketAddress;
import java.net.URLDecoder;
import java.nio.charset.StandardCharsets;
import java.security.KeyStore;
import java.util.*;
import java.util.concurrent.Executors;

public class AccountServer {
    private static final Gson GSON = new GsonBuilder().disableHtmlEscaping().create();
    private final JsonStore users = new JsonStore("users");
    private final JsonStore servers = new JsonStore("servers");
    private HttpsServer http;

    public void start() throws Exception {
        String password = System.getenv("XZJ_TLS_PASSWORD");
        if (password == null || password.isEmpty()) {
            throw new IllegalStateException("Set XZJ_TLS_PASSWORD before starting the server");
        }
        char[] pass = password.toCharArray();
        KeyStore ks = KeyStore.getInstance("PKCS12");
        try (InputStream in = new FileInputStream(Config.DATA_DIR + "/server.p12")) {
            ks.load(in, pass);
        }
        KeyManagerFactory kmf = KeyManagerFactory.getInstance("SunX509");
        kmf.init(ks, pass);
        SSLContext ssl = SSLContext.getInstance("TLS");
        ssl.init(kmf.getKeyManagers(), null, null);

        InetSocketAddress address = Config.listenAddress(Config.ACCOUNT_PORT);
        http = HttpsServer.create(address, 0);
        http.setHttpsConfigurator(new HttpsConfigurator(ssl));
        route("/login", this::login, "POST");
        route("/user/register", this::register, "POST");
        route("/user/sendCheckCode", this::sendCheckCode, "GET");
        route("/user/pwdUpdate", this::pwdUpdate, "POST");
        route("/user/roleSuicide", this::roleSuicide, "POST");
        route("/announcement/queryInfo", this::announcementQuery, "GET");
        route("/announcement/isShow", this::announcementIsShow, "GET");
        route("/gameService/statusSearch", this::statusSearch, "GET");
        route("/client/data/queryClientDataModel", this::clientDataModel, "GET");
        route("/client/log/error", this::clientLogError, "POST");
        route("/login/roleCdKey/cdkeyExchange", this::cdkeyExchange, "GET");
        route("/client/update/local/download", this::hotfixDownload, "GET");
        http.setExecutor(Executors.newCachedThreadPool());
        http.start();
        System.out.println("[AccountServer] listening on https://" + address.getHostString() + ":" + address.getPort());
    }

    private void route(String path, HttpHandler h, String method) {
        http.createContext(path, exchange -> {
            try {
                exchange.getResponseHeaders().set("Content-Type", "application/json; charset=utf-8");
                if (!exchange.getRequestMethod().equalsIgnoreCase(method)) {
                    respond(exchange, 200, error("1001", "method not allowed", null));
                    return;
                }
                h.handle(exchange);
            } catch (Exception e) {
                e.printStackTrace();
                respond(exchange, 200, error("1001", "server error", null));
            }
        });
    }

    private String ok(String dataString) {
        Map<String, Object> biz = new LinkedHashMap<>();
        biz.put("code", "2000");
        biz.put("msg", "success");
        biz.put("dataString", dataString);
        Map<String, Object> sys = new LinkedHashMap<>();
        sys.put("httpCode", 200);
        Map<String, Object> body = new LinkedHashMap<>();
        body.put("sysInfo", sys);
        body.put("bizInfo", biz);
        Map<String, Object> root = new LinkedHashMap<>();
        root.put("msgBody", body);
        return GSON.toJson(root);
    }

    private String error(String code, String msg, String dataString) {
        Map<String, Object> biz = new LinkedHashMap<>();
        biz.put("code", code);
        biz.put("msg", msg);
        biz.put("dataString", dataString == null ? "" : dataString);
        Map<String, Object> sys = new LinkedHashMap<>();
        sys.put("httpCode", 200);
        Map<String, Object> body = new LinkedHashMap<>();
        body.put("sysInfo", sys);
        body.put("bizInfo", biz);
        Map<String, Object> root = new LinkedHashMap<>();
        root.put("msgBody", body);
        return GSON.toJson(root);
    }

    private void respond(HttpExchange ex, int status, String body) throws IOException {
        byte[] b = body.getBytes(StandardCharsets.UTF_8);
        ex.sendResponseHeaders(status, b.length);
        try (OutputStream os = ex.getResponseBody()) {
            os.write(b);
        }
    }

    private Map<String, String> parseForm(HttpExchange ex) throws IOException {
        Map<String, String> map = new HashMap<>();
        String body = new String(readAll(ex.getRequestBody()), StandardCharsets.UTF_8);
        for (String kv : body.split("&")) {
            int i = kv.indexOf('=');
            if (i > 0) {
                map.put(URLDecoder.decode(kv.substring(0, i), "UTF-8"),
                        URLDecoder.decode(kv.substring(i + 1), "UTF-8"));
            }
        }
        return map;
    }

    private byte[] readAll(InputStream in) throws IOException {
        ByteArrayOutputStream bos = new ByteArrayOutputStream();
        byte[] buf = new byte[4096];
        int n;
        while ((n = in.read(buf)) > 0) bos.write(buf, 0, n);
        return bos.toByteArray();
    }

    @SuppressWarnings("unchecked")
    private Map<String, Object> userOf(String phone) {
        Map<String, Object> all = users.all();
        for (Map.Entry<String, Object> e : all.entrySet()) {
            Map<String, Object> u = (Map<String, Object>) e.getValue();
            if (phone.equals(u.get("mobilePhone"))) return u;
        }
        return null;
    }

    private Map<String, Object> serverInfo() {
        Map<String, Object> s = new LinkedHashMap<>();
        s.put("gameServerId", "1");
        s.put("name", "修真界-私服1");
        s.put("osType", 1);
        s.put("serverIp", ServerConfig.get().publicIp);
        s.put("port", Config.GAME_PORT);
        s.put("registerCount", 0);
        s.put("onlineCount", 0);
        s.put("maxCount", 5000);
        s.put("currentRoleVO", null);
        s.put("gameServerIpManageId", "1");
        s.put("status", 1);
        s.put("serverVersion", Config.SERVER_VERSION);
        return s;
    }

    @SuppressWarnings("unchecked")
    private Map<String, Object> findRoleOfUser(String userId) {
        JsonStore roles = new JsonStore("roles");
        for (Map.Entry<String, Object> e : roles.all().entrySet()) {
            Map<String, Object> r = (Map<String, Object>) e.getValue();
            if (userId.equals(r.get("UserId"))) return r;
        }
        return null;
    }

    private void login(HttpExchange ex) throws IOException {
        Map<String, String> form = parseForm(ex);
        String phone = form.get("mobilePhone");
        String pass = form.get("password");
        System.out.println("[AccountServer] /login " + phone + " ver=" + form.get("clientVersion"));
        Map<String, Object> user = userOf(phone);
        boolean newUser = false;
        if (user == null) {
            user = new LinkedHashMap<>();
            user.put("mobilePhone", phone);
            user.put("password", pass);
            user.put("userId", "u_" + System.currentTimeMillis());
            users.put("u_" + phone, user);
            newUser = true;
        } else if (!Objects.equals(user.get("password"), pass)) {
            respond(ex, 200, error("2101", "密码错误", null));
            return;
        }
        Map<String, Object> lm = new LinkedHashMap<>();
        lm.put("userId", user.get("userId"));
        lm.put("mobilePhone", phone);
        Map<String, Object> srv = serverInfo();
        Map<String, Object> role = findRoleOfUser((String) user.get("userId"));
        if (role != null) {
            Map<String, Object> rvo = new LinkedHashMap<>();
            rvo.put("id", role.get("Id"));
            rvo.put("userId", role.get("UserId"));
            rvo.put("serversRoleId", role.get("Id"));
            rvo.put("name", role.get("Name"));
            rvo.put("level", ((Number) role.get("Level")).intValue());
            rvo.put("gameServersId", "1");
            srv.put("currentRoleVO", rvo);
            System.out.println("[AccountServer] login with existing role: " + role.get("Name"));
        }
        lm.put("gameServersInfoList", Arrays.asList(srv));
        lm.put("lastGameServersId", "1");
        lm.put("whetherNewUser", newUser);
        respond(ex, 200, ok(GSON.toJson(lm)));
    }

    private void register(HttpExchange ex) throws IOException {
        Map<String, String> form = parseForm(ex);
        String phone = form.get("mobilePhone");
        String pass = form.get("password");
        System.out.println("[AccountServer] /user/register " + phone);
        if (userOf(phone) != null) {
            respond(ex, 200, error("2201", "手机号已注册", null));
            return;
        }
        Map<String, Object> user = new LinkedHashMap<>();
        user.put("mobilePhone", phone);
        user.put("password", pass);
        user.put("userId", "u_" + System.currentTimeMillis());
        users.put("u_" + phone, user);
        respond(ex, 200, ok("true"));
    }

    private void sendCheckCode(HttpExchange ex) throws IOException {
        System.out.println("[AccountServer] /user/sendCheckCode (fake sms ok)");
        respond(ex, 200, ok("true"));
    }

    private void pwdUpdate(HttpExchange ex) throws IOException {
        Map<String, String> form = parseForm(ex);
        String phone = form.get("mobilePhone");
        Map<String, Object> user = userOf(phone);
        if (user == null) {
            respond(ex, 200, error("2301", "账号不存在", null));
            return;
        }
        if (form.containsKey("password")) {
            Map<String, Object> updated = new LinkedHashMap<>(user);
            updated.put("password", form.get("password"));
            users.put("u_" + phone, updated);
        }
        respond(ex, 200, ok("true"));
    }

    private void roleSuicide(HttpExchange ex) throws IOException {
        System.out.println("[AccountServer] /user/roleSuicide");
        respond(ex, 200, ok("true"));
    }

    private void announcementQuery(HttpExchange ex) throws IOException {
        respond(ex, 200, ok("[]"));
    }

    private void announcementIsShow(HttpExchange ex) throws IOException {
        respond(ex, 200, ok("false"));
    }

    private void statusSearch(HttpExchange ex) throws IOException {
        Map<String, Object> s = serverInfo();
        s.put("registerCount", 10);
        s.put("onlineCount", 0);
        respond(ex, 200, ok(GSON.toJson(Arrays.asList(s))));
    }

    private void clientDataModel(HttpExchange ex) throws IOException {
        respond(ex, 200, ok("{}"));
    }

    private void clientLogError(HttpExchange ex) throws IOException {
        respond(ex, 200, ok("true"));
    }

    private void cdkeyExchange(HttpExchange ex) throws IOException {
        respond(ex, 200, error("2501", "兑换码无效", null));
    }

    private void hotfixDownload(HttpExchange ex) throws IOException {
        System.out.println("[AccountServer] /client/update/local/download " + ex.getRequestURI());
        // 客户端启动时会先拉 ServerInfo.xml。官服已关，这里返回一个“无热更”的合法 XML，
        // 避免客户端把 404 的 HTML 当 XML 解析而卡在加载界面。
        String xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>"
                + "<ServerInfo><GameVersion Version=\"" + Config.SERVER_VERSION + "\"></GameVersion></ServerInfo>";
        byte[] b = xml.getBytes(StandardCharsets.UTF_8);
        ex.getResponseHeaders().set("Content-Type", "application/xml; charset=utf-8");
        ex.sendResponseHeaders(200, b.length);
        try (OutputStream os = ex.getResponseBody()) {
            os.write(b);
        }
    }
}
