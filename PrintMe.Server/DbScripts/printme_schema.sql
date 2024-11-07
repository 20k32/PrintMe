CREATE TABLE IF NOT EXISTS user_status
(
    user_status_id SERIAL PRIMARY KEY,
    status         VARCHAR(50) NOT NULL
);

CREATE TABLE IF NOT EXISTS request_type
(
    request_type_id SERIAL PRIMARY KEY,
    type            VARCHAR(50) NOT NULL
);

CREATE TABLE IF NOT EXISTS request_status
(
    request_status_id SERIAL PRIMARY KEY,
    status            VARCHAR(50) NOT NULL
);

CREATE TABLE IF NOT EXISTS request_status_reason
(
    request_status_reason_id SERIAL PRIMARY KEY,
    reason                   VARCHAR(50) NOT NULL
);

CREATE TABLE IF NOT EXISTS print_order_status
(
    print_order_status_id SERIAL PRIMARY KEY,
    status                VARCHAR(50) NOT NULL
);

CREATE TABLE IF NOT EXISTS print_order_status_reason
(
    print_order_status_reason_id SERIAL PRIMARY KEY,
    reason                       VARCHAR(50) NOT NULL
);

CREATE TABLE IF NOT EXISTS "user"
(
    user_id                  SERIAL PRIMARY KEY,
    first_name               VARCHAR(255)        NOT NULL,
    last_name                VARCHAR(255)        NOT NULL,
    email                    VARCHAR(255) UNIQUE NOT NULL,
    phone_number             VARCHAR(20) UNIQUE,
    user_status_id           INT                 REFERENCES user_status (user_status_id) ON DELETE SET NULL,
    should_hide_phone_number BOOLEAN DEFAULT TRUE,
    description              VARCHAR(1024),
    password                 VARCHAR(255)        NOT NULL
);

CREATE TABLE IF NOT EXISTS rating
(
    rating_id SERIAL PRIMARY KEY,
    reviewer  INT REFERENCES "user" (user_id) ON DELETE CASCADE,
    target    INT REFERENCES "user" (user_id) ON DELETE CASCADE,
    value     INT CHECK (value BETWEEN 1 AND 10)
);

CREATE TABLE IF NOT EXISTS chat
(
    chat_id     SERIAL PRIMARY KEY,
    user1_id    INT REFERENCES "user" (user_id) ON DELETE CASCADE,
    user2_id    INT REFERENCES "user" (user_id) ON DELETE CASCADE,
    is_archived BOOLEAN DEFAULT FALSE
);

CREATE TABLE IF NOT EXISTS message
(
    chat_id   INT REFERENCES chat (chat_id) ON DELETE CASCADE,
    send_time TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    sender_id INT REFERENCES "user" (user_id) ON DELETE CASCADE,
    text      VARCHAR(1024),
    PRIMARY KEY (chat_id, send_time)
);

CREATE TABLE IF NOT EXISTS request
(
    request_id               SERIAL PRIMARY KEY,
    user_text_data           VARCHAR(1024),
    user_sender_id           INT NOT NULL REFERENCES "user" (user_id) ON DELETE CASCADE,
    request_type_id          INT NOT NULL REFERENCES request_type (request_type_id) ON DELETE CASCADE,
    reported_user_id         INT REFERENCES "user" (user_id) ON DELETE SET NULL,
    delete_user_id           INT REFERENCES "user" (user_id) ON DELETE SET NULL,
    model_id                 INT,
    description              VARCHAR(1024),
    location                 JSONB,
    min_model_height         FLOAT,
    min_model_width          FLOAT,
    max_model_height         FLOAT,
    max_model_width          FLOAT,
    request_status_id        INT NOT NULL REFERENCES request_status (request_status_id) ON DELETE CASCADE,
    request_status_reason_id INT REFERENCES request_status_reason (request_status_reason_id) ON DELETE SET NULL,
    print_materials_id_array INT[]
);

CREATE TABLE IF NOT EXISTS print_material
(
    print_material_id SERIAL PRIMARY KEY,
    name              VARCHAR(255) NOT NULL
);

CREATE TABLE IF NOT EXISTS printer_model
(
    printer_model_id SERIAL PRIMARY KEY,
    name             VARCHAR(255) NOT NULL
);

CREATE TABLE IF NOT EXISTS printer
(
    printer_id       SERIAL PRIMARY KEY,
    printer_model_id INT REFERENCES printer_model (printer_model_id) ON DELETE CASCADE,
    user_id          INT REFERENCES "user" (user_id) ON DELETE CASCADE,
    description      VARCHAR(1024),
    min_model_height FLOAT NOT NULL,
    min_model_width  FLOAT NOT NULL,
    max_model_height FLOAT NOT NULL,
    max_model_width  FLOAT NOT NULL,
    location         JSONB NOT NULL
);

CREATE TABLE IF NOT EXISTS print_materials
(
    printer_id  INT REFERENCES printer (printer_id) ON DELETE CASCADE,
    material_id INT REFERENCES print_material (print_material_id) ON DELETE CASCADE,
    PRIMARY KEY (printer_id, material_id)
);

CREATE TABLE IF NOT EXISTS print_order
(
    print_order_id               SERIAL PRIMARY KEY,
    user_id                      INT REFERENCES "user" (user_id) ON DELETE CASCADE,
    printer_id                   INT REFERENCES printer (printer_id) ON DELETE CASCADE,
    price                        DECIMAL(10, 2) NOT NULL,
    order_date                   DATE DEFAULT CURRENT_DATE,
    due_date                     DATE DEFAULT (CURRENT_DATE + INTERVAL '7 days'),
    item_link                    VARCHAR(1024)  NOT NULL,
    item_quantity                INT CHECK (item_quantity > 0),
    item_description             VARCHAR(1024),
    item_material_id             INT            REFERENCES print_material (print_material_id) ON DELETE SET NULL,
    print_order_status_id        INT            REFERENCES print_order_status (print_order_status_id) ON DELETE SET NULL,
    print_order_status_reason_id INT            REFERENCES print_order_status_reason (print_order_status_reason_id) ON DELETE SET NULL
);

CREATE TABLE IF NOT EXISTS favourites
(
    user_id    INT REFERENCES "user" (user_id) ON DELETE CASCADE,
    printer_id INT REFERENCES printer (printer_id) ON DELETE CASCADE,
    PRIMARY KEY (user_id, printer_id)
);

-- User table indexes
CREATE INDEX IF NOT EXISTS idx_user_email ON "user" (email);
CREATE INDEX IF NOT EXISTS idx_user_phone_number ON "user" (phone_number);
CREATE INDEX IF NOT EXISTS idx_user_status_id ON "user" (user_status_id);

-- Rating table indexes
CREATE INDEX IF NOT EXISTS idx_rating_reviewer ON rating (reviewer);
CREATE INDEX IF NOT EXISTS idx_rating_target ON rating (target);

-- Chat table indexes
CREATE INDEX IF NOT EXISTS idx_chat_user1_id ON chat (user1_id);
CREATE INDEX IF NOT EXISTS idx_chat_user2_id ON chat (user2_id);

-- Message table indexes
CREATE INDEX IF NOT EXISTS idx_message_chat_id ON message (chat_id);
CREATE INDEX IF NOT EXISTS idx_message_send_time ON message (send_time);

-- Request table indexes
CREATE INDEX IF NOT EXISTS idx_request_user_sender_id ON request (user_sender_id);
CREATE INDEX IF NOT EXISTS idx_request_request_type_id ON request (request_type_id);
CREATE INDEX IF NOT EXISTS idx_request_request_status_id ON request (request_status_id);

-- Print_material table indexes
CREATE INDEX IF NOT EXISTS idx_printer_user_id ON printer (user_id);
CREATE INDEX IF NOT EXISTS idx_printer_printer_model_id ON printer (printer_model_id);

-- Print_material table indexes
CREATE INDEX IF NOT EXISTS idx_print_order_user_id ON print_order (user_id);
CREATE INDEX IF NOT EXISTS idx_print_order_printer_id ON print_order (printer_id);
CREATE INDEX IF NOT EXISTS idx_print_order_order_date ON print_order (order_date);

-- Favourites table indexes
CREATE INDEX IF NOT EXISTS idx_favourites_user_id ON favourites (user_id);
CREATE INDEX IF NOT EXISTS idx_favourites_printer_id ON favourites (printer_id);

-- Foreign key constraints
ALTER TABLE rating
    ADD CONSTRAINT chk_rating_reviewer_target CHECK (reviewer != target);
ALTER TABLE chat
    ADD CONSTRAINT chk_chat_user1_user2 CHECK (user1_id != user2_id);

ALTER TABLE request
    ADD CONSTRAINT chk_request_min_max CHECK (min_model_height <= max_model_height AND min_model_width <= max_model_width);
ALTER TABLE print_order
    ADD CONSTRAINT chk_print_order_dates CHECK (order_date <= due_date);
ALTER TABLE print_materials
    ADD CONSTRAINT chk_print_materials_material_id CHECK (printer_id != material_id);
