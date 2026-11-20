-- SQLite 3.37+；只接受服务端确认完成的历练。秒数为 UTC Unix 时间。
PRAGMA foreign_keys = ON;
CREATE TABLE projection_run (
    run_id INTEGER PRIMARY KEY CHECK (run_id > 0),
    actor_id INTEGER NOT NULL CHECK (actor_id > 0),
    map_id INTEGER NOT NULL CHECK (map_id > 0),
    cleared_at INTEGER NOT NULL CHECK (cleared_at >= 0),
    expires_at INTEGER NOT NULL CHECK (expires_at > cleared_at),
    rule_id INTEGER NOT NULL CHECK (rule_id > 0),
    catalog_version TEXT NOT NULL CHECK (length(catalog_version)=64 AND catalog_version NOT GLOB '*[^0-9a-f]*'),
    public_name TEXT NOT NULL CHECK (length(trim(public_name)) BETWEEN 1 AND 48),
    level INTEGER NOT NULL CHECK (level > 0),
    hp INTEGER NOT NULL CHECK (hp > 0),
    energy INTEGER NOT NULL CHECK (energy >= 0),
    attack INTEGER NOT NULL CHECK (attack > 0),
    defense INTEGER NOT NULL CHECK (defense >= 0),
    speed INTEGER NOT NULL CHECK (speed > 0),
    spirit INTEGER NOT NULL CHECK (spirit >= 0),
    loadout TEXT NOT NULL CHECK (json_valid(loadout) AND json_type(loadout)='object'),
    UNIQUE (map_id, actor_id, run_id)
) STRICT;
CREATE TRIGGER projection_no_rewrite BEFORE UPDATE ON projection_run
BEGIN SELECT RAISE(ABORT,'run snapshot is immutable'); END;
CREATE TRIGGER projection_no_replace BEFORE INSERT ON projection_run
WHEN EXISTS (SELECT 1 FROM projection_run WHERE run_id = NEW.run_id)
BEGIN SELECT RAISE(ABORT,'run already recorded'); END;

-- 同一人在同图只占一席。晚到的旧战报不能顶掉新投影。停用可撤下候选席位。
CREATE TABLE projection_pool (
    map_id INTEGER NOT NULL,
    actor_id INTEGER NOT NULL,
    run_id INTEGER NOT NULL UNIQUE,
    seat INTEGER NOT NULL CHECK (seat >= 1),
    expires_at INTEGER NOT NULL,
    UNIQUE (map_id,seat),
    PRIMARY KEY (map_id, actor_id),
    FOREIGN KEY (map_id,actor_id,run_id) REFERENCES projection_run(map_id,actor_id,run_id)
) STRICT, WITHOUT ROWID;
CREATE INDEX projection_pool_expiry ON projection_pool(map_id,expires_at);
CREATE TABLE projection_encounter (
    encounter_id INTEGER PRIMARY KEY CHECK (encounter_id > 0),
    viewer_id INTEGER NOT NULL CHECK (viewer_id > 0),
    map_id INTEGER NOT NULL CHECK (map_id > 0),
    drawn_at INTEGER NOT NULL CHECK (drawn_at >= 0),
    run_id INTEGER REFERENCES projection_run,
    reward_pool INTEGER,
    reward_tier INTEGER,
    reward_catalog TEXT,
    CHECK ((run_id IS NULL AND reward_pool IS NULL AND reward_tier IS NULL AND reward_catalog IS NULL)
        OR (run_id IS NOT NULL AND reward_pool IS NOT NULL AND reward_pool > 0 AND reward_tier IS NOT NULL
            AND reward_tier IN (1,3,4) AND reward_catalog IS NOT NULL AND length(reward_catalog) = 64)),
    UNIQUE (encounter_id,run_id)
) STRICT;
-- 胜利结算回执也保存零掉落，重复结算不重抽。每个奖励位最多发一次。
CREATE TABLE projection_claim (
    encounter_id INTEGER PRIMARY KEY,
    run_id INTEGER NOT NULL,
    FOREIGN KEY (encounter_id,run_id) REFERENCES projection_encounter(encounter_id,run_id),
    UNIQUE (encounter_id,run_id)
) STRICT;
CREATE TABLE projection_grant (
    encounter_id INTEGER NOT NULL,
    run_id INTEGER NOT NULL,
    reward_slot INTEGER NOT NULL CHECK (reward_slot > 0),
    kind_id INTEGER NOT NULL CHECK (kind_id > 0),
    template_id INTEGER NOT NULL CHECK (template_id > 0),
    revision INTEGER NOT NULL CHECK (revision > 0),
    quantity INTEGER NOT NULL CHECK (quantity > 0),
    embedded_kind INTEGER,
    embedded_id INTEGER,
    embedded_revision INTEGER,
    CHECK ((embedded_kind IS NULL AND embedded_id IS NULL AND embedded_revision IS NULL)
        OR (embedded_kind IS NOT NULL AND embedded_id IS NOT NULL AND embedded_revision IS NOT NULL)),
    PRIMARY KEY (encounter_id,reward_slot),
    FOREIGN KEY (encounter_id,run_id) REFERENCES projection_claim(encounter_id,run_id),
    FOREIGN KEY (kind_id,template_id,revision) REFERENCES content_revision,
    FOREIGN KEY (embedded_kind,embedded_id,embedded_revision) REFERENCES content_revision
) STRICT, WITHOUT ROWID;
CREATE INDEX projection_run_expiry ON projection_run (expires_at);
CREATE INDEX projection_encounter_viewer ON projection_encounter(viewer_id,drawn_at);
