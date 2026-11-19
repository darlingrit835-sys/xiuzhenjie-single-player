package server.common;

import java.net.InetSocketAddress;

public class Config {
    /** 单机版：客户端与本地服务端同机/经 adb reverse 直连回环地址 */
    public static final String PUBLIC_IP = System.getProperty("xzj.publicIp", "127.0.0.1");
    public static final int ACCOUNT_PORT = 9443;
    public static final int GAME_PORT = 17666;
    public static final String DATA_DIR = System.getProperty("xzj.dataDir", System.getProperty("user.dir") + "/data");
    public static final String SERVER_VERSION = "1.3.5";

    public static InetSocketAddress listenAddress(int port) {
        String address = System.getProperty("xzj.bindAddress", "");
        return address.isEmpty() ? new InetSocketAddress(port) : new InetSocketAddress(address, port);
    }
}
