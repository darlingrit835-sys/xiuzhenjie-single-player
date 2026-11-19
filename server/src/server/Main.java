package server;

import server.account.AccountServer;
import server.game.BattleServer;
import server.game.GameServer;

public class Main {
    public static void main(String[] args) {
        try {
            new AccountServer().start();
            Thread battle = new Thread(() -> new BattleServer().start(), "battle-server");
            battle.setUncaughtExceptionHandler((thread, error) -> {
                error.printStackTrace();
                System.exit(1);
            });
            battle.start();
            new GameServer().start();
        } catch (Exception e) {
            e.printStackTrace();
            System.exit(1);
        }
    }
}
