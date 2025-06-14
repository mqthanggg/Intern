BEGIN;

SET search_path TO petro_application, public;

ALTER TABLE IF EXISTS petro_application.dispenser DROP CONSTRAINT IF EXISTS None;

ALTER TABLE IF EXISTS petro_application.dispenser DROP CONSTRAINT IF EXISTS None;

ALTER TABLE IF EXISTS petro_application.dispenser DROP CONSTRAINT IF EXISTS None;

ALTER TABLE IF EXISTS petro_application.tank DROP CONSTRAINT IF EXISTS None;

ALTER TABLE IF EXISTS petro_application.tank DROP CONSTRAINT IF EXISTS None;

ALTER TABLE IF EXISTS petro_application.log DROP CONSTRAINT IF EXISTS None;

DROP TABLE IF EXISTS petro_application.dispenser CASCADE;

CREATE TABLE IF NOT EXISTS petro_application.dispenser
(
    dispenser_id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 ),
    station_id integer NOT NULL,
    tank_id integer NOT NULL,
    fuel_id integer NOT NULL,
    name integer NOT NULL,
    created_by character varying(255),
    created_date timestamp(0) DEFAULT now(),
    last_modified_by character varying(255),
    last_modified_date timestamp(0) DEFAULT now(),
    PRIMARY KEY (dispenser_id)
        INCLUDE(fuel_id, tank_id),
    UNIQUE (tank_id)
);

COMMENT ON TABLE petro_application.dispenser
    IS 'Table for storing each dispenser''s information.';

DROP TABLE IF EXISTS petro_application.station CASCADE;

CREATE TABLE IF NOT EXISTS petro_application.station
(
    station_id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 ),
    name character varying(255) NOT NULL,
    address character varying(255) NOT NULL,
    created_by character varying(255),
    created_date timestamp(0) DEFAULT now(),
    last_modified_by character varying(255),
    last_modified_date timestamp(0) DEFAULT now(),
    PRIMARY KEY (station_id)
);

COMMENT ON TABLE petro_application.station
    IS 'Table for storing each station''s information.';

DROP TABLE IF EXISTS petro_application.fuel CASCADE;

CREATE TABLE IF NOT EXISTS petro_application.fuel
(
    fuel_id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 ),
    short_name character(3) NOT NULL,
    long_name character varying(15) NOT NULL,
    price integer NOT NULL,
    created_by character varying(255),
    created_date timestamp(0) DEFAULT now(),
    last_modified_by character varying(255),
    last_modified_date timestamp(0) DEFAULT now(),
    PRIMARY KEY (fuel_id)
);

COMMENT ON TABLE petro_application.fuel
    IS 'Table for storing fuel''s information.';

DROP TABLE IF EXISTS petro_application.tank CASCADE;

CREATE TABLE IF NOT EXISTS petro_application.tank
(
    tank_id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 ),
    fuel_id integer NOT NULL,
    station_id integer NOT NULL,
    name integer NOT NULL,
    max_volume integer NOT NULL,
    created_by character varying(255),
    created_date timestamp(0) DEFAULT now(),
    last_modified_by character varying(255),
    last_modified_date timestamp(0) DEFAULT now(),
    PRIMARY KEY (tank_id)
);

COMMENT ON TABLE petro_application.tank
    IS 'Table for storing each tank''s information.';

DROP TABLE IF EXISTS petro_application.user CASCADE;

CREATE TABLE IF NOT EXISTS petro_application.user
(
    user_id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 ),
    username character varying(15) NOT NULL UNIQUE,
    password character(84) NOT NULL,
    padding character(24) NOT NULL UNIQUE,
    refresh_token character(84) UNIQUE,
    token_padding character(24) UNIQUE,
    token_expired_time timestamp(0),
    created_by character varying(255),
    created_date timestamp(0) DEFAULT now(),
    last_modified_by character varying(255),
    last_modified_date timestamp(0) DEFAULT now(),
    PRIMARY KEY (user_id)
);

COMMENT ON TABLE petro_application."user"
    IS 'Table for storing user''s credentials.';

DROP TABLE IF EXISTS petro_application.log CASCADE;

CREATE TABLE IF NOT EXISTS petro_application.log
(
    log_id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 ),
    dispenser_id integer NOT NULL,
    fuel_name character varying(15) NOT NULL,
    total_liters real NOT NULL,
    total_amount integer NOT NULL,
    time timestamp(0) without time zone NOT NULL DEFAULT now(),
    created_by character varying(255),
    created_date timestamp(0) DEFAULT now(),
    last_modified_by character varying(255),
    last_modified_date timestamp(0) DEFAULT now(),
    PRIMARY KEY (log_id)
);

COMMENT ON TABLE petro_application.log
    IS 'Table for storing logs.';

ALTER TABLE IF EXISTS petro_application.dispenser
    ADD FOREIGN KEY (station_id)
    REFERENCES petro_application.station (station_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION
    NOT VALID;


ALTER TABLE IF EXISTS petro_application.dispenser
    ADD FOREIGN KEY (fuel_id)
    REFERENCES petro_application.fuel (fuel_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION
    NOT VALID;


ALTER TABLE IF EXISTS petro_application.dispenser
    ADD FOREIGN KEY (tank_id)
    REFERENCES petro_application.tank (tank_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION
    NOT VALID;


ALTER TABLE IF EXISTS petro_application.tank
    ADD FOREIGN KEY (fuel_id)
    REFERENCES petro_application.fuel (fuel_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION
    NOT VALID;


ALTER TABLE IF EXISTS petro_application.tank
    ADD FOREIGN KEY (station_id)
    REFERENCES petro_application.station (station_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION
    NOT VALID;


ALTER TABLE IF EXISTS petro_application.log
    ADD FOREIGN KEY (dispenser_id)
    REFERENCES petro_application.dispenser (dispenser_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION
    NOT VALID;

CREATE OR REPLACE FUNCTION update_modified_fields()
RETURNS TRIGGER AS $$
BEGIN
  NEW.last_modified_date = CURRENT_TIMESTAMP;
  NEW.last_modified_by = CURRENT_USER;
  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION update_modified_fields_when_create()
RETURNS TRIGGER AS $$
BEGIN
  NEW.created_date = CURRENT_TIMESTAMP;
  NEW.created_by = CURRENT_USER;
  NEW.last_modified_date = CURRENT_TIMESTAMP;
  NEW.last_modified_by = CURRENT_USER;
  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER update_dispenser_audit_fields
BEFORE UPDATE ON petro_application.dispenser
FOR EACH ROW
EXECUTE FUNCTION update_modified_fields();

CREATE OR REPLACE TRIGGER update_dispenser_audit_fields_when_create
BEFORE INSERT ON petro_application.dispenser
FOR EACH ROW
EXECUTE FUNCTION update_modified_fields_when_create();

CREATE OR REPLACE TRIGGER update_fuel_audit_fields
BEFORE UPDATE ON petro_application.fuel
FOR EACH ROW
EXECUTE FUNCTION update_modified_fields();

CREATE OR REPLACE TRIGGER update_fuel_audit_fields_when_create
BEFORE INSERT ON petro_application.fuel
FOR EACH ROW
EXECUTE FUNCTION update_modified_fields_when_create();

CREATE OR REPLACE TRIGGER update_tank_audit_fields
BEFORE UPDATE ON petro_application.tank
FOR EACH ROW
EXECUTE FUNCTION update_modified_fields();

CREATE OR REPLACE TRIGGER update_tank_audit_fields_when_create
BEFORE INSERT ON petro_application.tank
FOR EACH ROW
EXECUTE FUNCTION update_modified_fields_when_create();

CREATE OR REPLACE TRIGGER update_station_audit_fields
BEFORE UPDATE ON petro_application.station
FOR EACH ROW
EXECUTE FUNCTION update_modified_fields();

CREATE OR REPLACE TRIGGER update_station_audit_fields_when_create
BEFORE INSERT ON petro_application.station
FOR EACH ROW
EXECUTE FUNCTION update_modified_fields_when_create();

CREATE OR REPLACE TRIGGER update_user_audit_fields
BEFORE UPDATE ON petro_application.user
FOR EACH ROW
EXECUTE FUNCTION update_modified_fields();

CREATE OR REPLACE TRIGGER update_user_audit_fields_when_create
BEFORE INSERT ON petro_application.user
FOR EACH ROW
EXECUTE FUNCTION update_modified_fields_when_create();

CREATE OR REPLACE TRIGGER update_log_audit_fields
BEFORE UPDATE ON petro_application.log
FOR EACH ROW
EXECUTE FUNCTION update_modified_fields();

CREATE OR REPLACE TRIGGER update_log_audit_fields_when_create
BEFORE INSERT ON petro_application.log
FOR EACH ROW
EXECUTE FUNCTION update_modified_fields_when_create();

INSERT INTO petro_application.station (name, address) VALUES
('Petrolimex Station 1', '123 Nguyen Hue, Ben Nghe Ward, District 1, Ho Chi Minh City'),
('PV Oil Station 2', '456 Cach Mang Thang 8, Ward 5, District 3, Ho Chi Minh City'),
('Saigon Petro Station 3', '789 Tran Hung Dao, Ward 6, District 5, Ho Chi Minh City'),
('COMECO Station 4', '101 Le Van Luong, Tan Phong Ward, District 7, Ho Chi Minh City'),
('Mipec Station 5', '234 Vo Van Tan, Ward 11, District 10, Ho Chi Minh City'),
('Petrolimex Station 6', '567 Xa Lo Ha Noi, An Phu Ward, District 2, Ho Chi Minh City'),
('PV Oil Station 7', '890 Hoang Dieu, Ward 9, District 4, Ho Chi Minh City'),
('Saigon Petro Station 8', '321 Pham Van Dong, Linh Trung Ward, Thu Duc City, Ho Chi Minh City'),
('COMECO Station 9', '654 Phan Van Tri, Ward 11, Go Vap District, Ho Chi Minh City'),
('Mipec Station 10', '987 Ba Hom, Binh Tri Dong Ward, Binh Tan District, Ho Chi Minh City');

INSERT INTO petro_application.fuel (short_name, long_name, price) VALUES
('A95', 'RON 95-I', 25000),
('E5', 'E5 RON 92-II', 23000),
('DO1', 'Diesel Oil-I', 20000),
('DO5', 'Diesel Oil-V', 19000);

INSERT INTO petro_application.tank (fuel_id, station_id, name, max_volume) VALUES
(1, 1, 101, 5000), (2, 1, 102, 4000), (3, 1, 103, 6000), (4, 1, 104, 5500), (1, 1, 105, 7000),
(2, 2, 201, 5000), (3, 2, 202, 7500), (4, 2, 203, 6800), (1, 2, 204, 6000), (2, 2, 205, 4500),
(3, 3, 301, 7000), (4, 3, 302, 7200), (1, 3, 303, 8000), (2, 3, 304, 5500), (3, 3, 305, 6200),
(4, 4, 401, 7700), (1, 4, 402, 5200), (2, 4, 403, 4100), (3, 4, 404, 6300), (4, 4, 405, 5600);

INSERT INTO petro_application.dispenser (station_id, tank_id, fuel_id, name) VALUES
(1, 1, 1, 101), (1, 2, 2, 102), (1, 3, 3, 103), (1, 4, 4, 104), (1, 5, 1, 105),
(2, 6, 2, 201), (2, 7, 3, 202), (2, 8, 4, 203), (2, 9, 1, 204), (2, 10, 2, 205),
(3, 11, 3, 301), (3, 12, 4, 302), (3, 13, 1, 303), (3, 14, 2, 304), (3, 15, 3, 305),
(4, 16, 4, 401), (4, 17, 1, 402), (4, 18, 2, 403), (4, 19, 3, 404), (4, 20, 4, 405);

INSERT INTO petro_application.log (dispenser_id, fuel_name, total_liters, total_amount, time) VALUES
(1, 'RON 95-I', 10.5, 26250, TIMESTAMP(0) '2025-06-04 14:30:00'),
(1, 'RON 95-I', 8.2, 20500, TIMESTAMP(0) '2025-06-04 14:45:00'),
(1, 'RON 95-I', 12.0, 30000, TIMESTAMP(0) '2025-06-04 15:00:00'),

(2, 'E5 RON 92-II', 9.5, 21850, TIMESTAMP(0) '2025-06-04 14:35:00'),
(2, 'E5 RON 92-II', 7.8, 17940, TIMESTAMP(0) '2025-06-04 14:50:00'),
(2, 'E5 RON 92-II', 11.2, 25760, TIMESTAMP(0) '2025-06-04 15:05:00'),

(3, 'Diesel Oil-I', 15.0, 30000, TIMESTAMP(0) '2025-06-04 14:40:00'),
(3, 'Diesel Oil-I', 13.2, 26400, TIMESTAMP(0) '2025-06-04 14:55:00'),
(3, 'Diesel Oil-I', 16.5, 33000, TIMESTAMP(0) '2025-06-04 15:10:00'),
(4, 'Diesel Oil-V', 12.3, 23370, TIMESTAMP(0) '2025-06-04 14:45:00'),
(4, 'Diesel Oil-V', 10.8, 20520, TIMESTAMP(0) '2025-06-04 15:00:00'),
(4, 'Diesel Oil-V', 14.5, 27550, TIMESTAMP(0) '2025-06-04 15:15:00');

DO $$ 
BEGIN 
    IF EXISTS (SELECT FROM pg_roles WHERE rolname = 'read_user') THEN 
        EXECUTE 'REVOKE USAGE ON SCHEMA petro_application FROM read_user';
		EXECUTE 'REVOKE ALL PRIVILEGES ON ALL TABLES IN SCHEMA petro_application FROM read_user';
    END IF; 
	IF EXISTS (SELECT FROM pg_roles WHERE rolname = 'write_user') THEN 
        EXECUTE 'REVOKE USAGE ON SCHEMA petro_application FROM write_user';
		EXECUTE 'REVOKE ALL PRIVILEGES ON ALL TABLES IN SCHEMA petro_application FROM write_user';
    END IF; 
END $$;

DROP USER IF EXISTS read_user;
CREATE USER read_user WITH ENCRYPTED PASSWORD 'read123';
GRANT USAGE ON SCHEMA petro_application TO read_user;
GRANT SELECT ON ALL TABLES IN SCHEMA petro_application TO read_user;

DROP USER IF EXISTS write_user;
CREATE USER write_user WITH ENCRYPTED PASSWORD 'write123';
GRANT USAGE ON SCHEMA petro_application TO write_user;
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA petro_application TO write_user;

END;