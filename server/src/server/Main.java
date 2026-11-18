package server;

import server.account.AccountServer;
import server.game.BattleServer;
import server.game.GameServer;

public class Main {
    public static void main(String[] args) {
        try {
            new Thread(() -> {
                try {
                    new AccountServer().start();
                } catch (Exception e) {
                    System.err.println("[AccountServer] failed to start:");
                    e.printStackTrace();
                    System.exit(1);
                }
            }, "account-server").start();
            new Thread(() -> new BattleServer().start(), "battle-server").start();
            new GameServer().start();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
