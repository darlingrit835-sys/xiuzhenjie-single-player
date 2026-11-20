-- 玩家创作存档草案。SQLite 3.37+。每个连接开启外键；生效前由服务端校验门类专属数据。
PRAGMA foreign_keys = ON;

CREATE TABLE content_kind (
    kind_id INTEGER PRIMARY KEY CHECK (kind_id > 0),
    creatable INTEGER NOT NULL CHECK (creatable IN (0, 1)),
    needs_prototype INTEGER NOT NULL CHECK (needs_prototype IN (0, 1))
) STRICT;

-- 内置目录键由总表分配；玩家 ID 在事务中分配。ID 永久保留。
CREATE TABLE content_template (
    kind_id INTEGER NOT NULL REFERENCES content_kind,
    template_id INTEGER NOT NULL CHECK (template_id BETWEEN 1 AND 2147483647),
    author_id INTEGER NOT NULL CHECK (author_id >= 0),
    CHECK ((template_id < 1000000000) = (author_id = 0)),
    PRIMARY KEY (kind_id, template_id)
) STRICT, WITHOUT ROWID;

-- 每版单存中文名、规则包哈希和完整配置快照。状态 0 草稿、1 已发布、2 已下架。
-- 旧版查阅与交易转手使用同一三元组，状态 2 保留已持有实例的解析能力。
CREATE TABLE content_revision (
    kind_id INTEGER NOT NULL,
    template_id INTEGER NOT NULL,
    revision INTEGER NOT NULL CHECK (revision > 0),
    state INTEGER NOT NULL DEFAULT 0 CHECK (state BETWEEN 0 AND 2),
    name TEXT NOT NULL CHECK (length(trim(name)) BETWEEN 1 AND 48),
    catalog_version TEXT NOT NULL CHECK (length(catalog_version) = 64 AND catalog_version NOT GLOB '*[^0-9a-f]*'),
    payload TEXT NOT NULL CHECK (json_valid(payload) AND json_type(payload) = 'object'),
    PRIMARY KEY (kind_id, template_id, revision),
    FOREIGN KEY (kind_id, template_id) REFERENCES content_template
) STRICT, WITHOUT ROWID;

-- 引用在发布前固定到一个已发布版本。原型同门类，跨门类使用普通依赖。
CREATE TABLE content_reference (
    kind_id INTEGER NOT NULL,
    template_id INTEGER NOT NULL,
    revision INTEGER NOT NULL,
    relation INTEGER NOT NULL CHECK (relation IN (1, 2)),
    target_kind INTEGER NOT NULL,
    target_template INTEGER NOT NULL,
    target_revision INTEGER NOT NULL,
    CHECK (kind_id != target_kind OR template_id != target_template OR target_revision < revision),
    CHECK (relation != 1 OR target_kind = kind_id),
    PRIMARY KEY (kind_id, template_id, revision, relation, target_kind, target_template, target_revision),
    FOREIGN KEY (kind_id, template_id, revision) REFERENCES content_revision,
    FOREIGN KEY (target_kind, target_template, target_revision) REFERENCES content_revision
) STRICT, WITHOUT ROWID;
CREATE UNIQUE INDEX content_one_prototype ON content_reference (kind_id, template_id, revision) WHERE relation = 1;
CREATE INDEX content_used_by ON content_reference (target_kind, target_template, target_revision);
CREATE INDEX content_by_author ON content_template (author_id, kind_id);
CREATE INDEX content_published ON content_revision (kind_id, template_id, revision DESC) WHERE state = 1;

CREATE TRIGGER content_kind_closed BEFORE INSERT ON content_template
WHEN NEW.author_id > 0 AND (SELECT creatable FROM content_kind WHERE kind_id = NEW.kind_id) = 0
BEGIN SELECT RAISE(ABORT, 'kind closed to players'); END;
CREATE TRIGGER content_no_replace BEFORE INSERT ON content_template
WHEN EXISTS (SELECT 1 FROM content_template WHERE (kind_id,template_id)=(NEW.kind_id,NEW.template_id))
BEGIN SELECT RAISE(ABORT, 'template identity already exists'); END;
CREATE TRIGGER content_identity_stable BEFORE UPDATE ON content_template
BEGIN SELECT RAISE(ABORT, 'template identity is permanent'); END;
CREATE TRIGGER content_no_id_reuse BEFORE DELETE ON content_template
BEGIN SELECT RAISE(ABORT, 'template IDs are retained'); END;

CREATE TRIGGER content_new_revision BEFORE INSERT ON content_revision
WHEN NEW.state != 0 OR NEW.revision != 1 + COALESCE((
    SELECT MAX(revision) FROM content_revision WHERE kind_id = NEW.kind_id AND template_id = NEW.template_id), 0)
BEGIN SELECT RAISE(ABORT, 'create the next revision as a draft'); END;
CREATE TRIGGER content_keep_revision BEFORE DELETE ON content_revision
BEGIN SELECT RAISE(ABORT, 'revision is retained'); END;
CREATE TRIGGER content_revision_identity BEFORE UPDATE OF kind_id,template_id,revision ON content_revision
BEGIN SELECT RAISE(ABORT, 'revision identity is permanent'); END;
CREATE TRIGGER content_revision_frozen BEFORE UPDATE OF name,catalog_version,payload ON content_revision
WHEN OLD.state != 0
BEGIN SELECT RAISE(ABORT, 'published snapshot is immutable'); END;
CREATE TRIGGER content_revision_transition BEFORE UPDATE OF state ON content_revision
WHEN NEW.state != OLD.state + 1
BEGIN SELECT RAISE(ABORT, 'invalid publication transition'); END;
CREATE TRIGGER content_reference_insert BEFORE INSERT ON content_reference
WHEN (SELECT state FROM content_revision WHERE (kind_id,template_id,revision) =
        (NEW.kind_id,NEW.template_id,NEW.revision)) != 0
  OR (SELECT state FROM content_revision WHERE (kind_id,template_id,revision) =
        (NEW.target_kind,NEW.target_template,NEW.target_revision)) != 1
BEGIN SELECT RAISE(ABORT, 'draft must refer to published content'); END;
CREATE TRIGGER content_reference_delete BEFORE DELETE ON content_reference
WHEN (SELECT state FROM content_revision WHERE (kind_id,template_id,revision) =
        (OLD.kind_id,OLD.template_id,OLD.revision)) != 0
BEGIN SELECT RAISE(ABORT, 'published references are immutable'); END;
CREATE TRIGGER content_reference_edit BEFORE UPDATE ON content_reference
BEGIN SELECT RAISE(ABORT, 'replace a draft reference by delete and insert'); END;
CREATE TRIGGER content_publish BEFORE UPDATE OF state ON content_revision
WHEN NEW.state = 1 AND (
    ((SELECT author_id FROM content_template WHERE kind_id = NEW.kind_id AND template_id = NEW.template_id) > 0
      AND (SELECT needs_prototype FROM content_kind WHERE kind_id = NEW.kind_id) = 1
      AND NOT EXISTS (SELECT 1 FROM content_reference WHERE (kind_id,template_id,revision) =
        (NEW.kind_id,NEW.template_id,NEW.revision) AND relation = 1))
    OR EXISTS (SELECT 1 FROM content_reference AS r JOIN content_revision AS t
        ON (t.kind_id,t.template_id,t.revision) = (r.target_kind,r.target_template,r.target_revision)
        WHERE (r.kind_id,r.template_id,r.revision) = (NEW.kind_id,NEW.template_id,NEW.revision) AND t.state != 1))
BEGIN SELECT RAISE(ABORT, 'publish requires active dependencies and a player prototype'); END;
