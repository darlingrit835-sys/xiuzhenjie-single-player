package server;

import server.account.AccountServer;
import server.game.BattleServer;
import server.game.GameServer;

public class Main {
    public static void main(String[] args) {
        startRetry("AccountServer", () -> new AccountServer().start());
        startRetry("BattleServer", () -> new BattleServer().start());
        startRetry("GameServer", () -> new GameServer().start());
    }

    /**
     * 每个子服务在自己线程里带重试启动。
     * 以前账号服一旦抛异常就 System.exit(1)，会把整个进程带走，
     * 玩家侧表现就是「游戏突然连不上服务器」，而且日志里只留一行 bind 错误。
     */
    private static void startRetry(String name, ThrowingRunnable starter) {
        Thread t = new Thread(() -> {
            for (int i = 1; i <= 5; i++) {
                try {
                    starter.run();
                    System.out.println("[" + name + "] stopped");
                    return;
                } catch (Exception e) {
                    System.err.println("[" + name + "] start failed (try " + i + "/5): " + e);
                    e.printStackTrace();
                }
                try {
                    Thread.sleep(2000);
                } catch (InterruptedException ie) {
                    return;
                }
            }
            System.err.println("[" + name + "] gave up after 5 tries");
        }, name);
        t.start();
    }

    private interface ThrowingRunnable {
        void run() throws Exception;
    }
}
