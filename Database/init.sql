BEGIN;

SET search_path TO petro_application, public;

ALTER TABLE IF EXISTS petro_application.assignment DROP CONSTRAINT IF EXISTS assignment_shift_id_fkey;

ALTER TABLE IF EXISTS petro_application.assignment DROP CONSTRAINT IF EXISTS assignment_staff_id_fkey;

ALTER TABLE IF EXISTS petro_application.assignment DROP CONSTRAINT IF EXISTS assignment_station_id_fkey;

ALTER TABLE IF EXISTS petro_application.dispenser DROP CONSTRAINT IF EXISTS None;

ALTER TABLE IF EXISTS petro_application.dispenser DROP CONSTRAINT IF EXISTS None;

ALTER TABLE IF EXISTS petro_application.dispenser DROP CONSTRAINT IF EXISTS None;

ALTER TABLE IF EXISTS petro_application.tank DROP CONSTRAINT IF EXISTS None;

ALTER TABLE IF EXISTS petro_application.tank DROP CONSTRAINT IF EXISTS None;

ALTER TABLE IF EXISTS petro_application.log DROP CONSTRAINT IF EXISTS None;

DROP TABLE IF EXISTS petro_application.assignment;

CREATE TABLE IF NOT EXISTS petro_application.assignment
(
    assignment_id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 ),
    shift_id integer NOT NULL,
    staff_id integer NOT NULL,
    station_id integer NOT NULL ,
	work_date date NOT NULL, 
    created_by character varying(255) DEFAULT now(),
    created_date timestamp(0) with time zone,
    last_modified_by character varying(255),
    last_modified_date timestamp(0) with time zone DEFAULT now(),
    CONSTRAINT assignment_pkey PRIMARY KEY (assignment_id)
);

COMMENT ON TABLE petro_application.assignment
    IS 'for staff '' work schedule in each station';

DROP TABLE IF EXISTS petro_application.shift;

CREATE TABLE IF NOT EXISTS petro_application.shift
(
    shift_id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 ),
    shift_type integer NOT NULL,
    CHECK (shift_type IN (1,2,3)),
    start_time time without time zone NOT NULL,
    end_time time without time zone NOT NULL,
    created_by character varying(255),
    created_date timestamp(0) with time zone DEFAULT now(),
    last_modified_by character varying(255),
    last_modified_date timestamp(0) with time zone DEFAULT now(),
    CONSTRAINT shift_pkey PRIMARY KEY (shift_id)
);

COMMENT ON TABLE petro_application.shift
    IS 'for shiff''s information. shift_type: 1 -> sang, 2 -> trua, 3 -> toi';

DROP TABLE IF EXISTS petro_application.staff;

CREATE TABLE IF NOT EXISTS petro_application.staff
(
    staff_id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 ),
    staff_name character varying(30) NOT NULL,
    date_birth date NOT NULL,
    phone character varying(10) NOT NULL,
    address character varying(50) NOT NULL,
    email character varying(255) NOT NULL,
    created_by character varying(255),
    created_date timestamp(0) with time zone DEFAULT now(),
    last_modified_by character varying(255),
    last_modified_date timestamp(0) with time zone DEFAULT now(),
    CONSTRAINT staff_pkey PRIMARY KEY (staff_id)
);

COMMENT ON TABLE petro_application.staff
    IS 'for staff''s information';

DROP TABLE IF EXISTS petro_application.dispenser CASCADE;

CREATE TABLE IF NOT EXISTS petro_application.dispenser
(
    dispenser_id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 ),
    station_id integer NOT NULL,
    tank_id integer NOT NULL,
    fuel_id integer NOT NULL,
    name integer NOT NULL,
    created_by character varying(255),
    created_date timestamp(0) with time zone DEFAULT now(),
    last_modified_by character varying(255),
    last_modified_date timestamp(0) with time zone DEFAULT now(),
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
    created_date timestamp(0) with time zone DEFAULT now(),
    last_modified_by character varying(255),
    last_modified_date timestamp(0) with time zone DEFAULT now(),
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
    created_date timestamp(0) with time zone DEFAULT now(),
    last_modified_by character varying(255),
    last_modified_date timestamp(0) with time zone DEFAULT now(),
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
    created_date timestamp(0) with time zone DEFAULT now(),
    last_modified_by character varying(255),
    last_modified_date timestamp(0) with time zone DEFAULT now(),
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
    token_expired_time timestamp(0) with time zone,
    created_by character varying(255),
    created_date timestamp(0) with time zone DEFAULT now(),
    last_modified_by character varying(255),
    last_modified_date timestamp(0) with time zone DEFAULT now(),
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
    log_type integer NOT NULL,
    CHECK (log_type IN (1,2,3,4)),
    created_by character varying(255),
    created_date timestamp(0) with time zone DEFAULT now(),
    last_modified_by character varying(255),
    last_modified_date timestamp(0) with time zone DEFAULT now(),
    PRIMARY KEY (log_id)
);

COMMENT ON TABLE petro_application.log
    IS 'Table for storing logs. log_type: 1 -> ban le, 2 -> cong no, 3 -> khuyen mai, 4 -> tra truoc';

ALTER TABLE IF EXISTS petro_application.assignment
    ADD CONSTRAINT assignment_shift_id_fkey FOREIGN KEY(shift_id)
    REFERENCES petro_application.shift (shift_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;


ALTER TABLE IF EXISTS petro_application.assignment
    ADD CONSTRAINT assignment_staff_id_fkey FOREIGN KEY (staff_id)
    REFERENCES petro_application.staff (staff_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;


ALTER TABLE IF EXISTS petro_application.assignment
    ADD CONSTRAINT assignment_station_id_fkey FOREIGN KEY (station_id)
    REFERENCES petro_application.station (station_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;

ALTER TABLE IF EXISTS petro_application.dispenser
    ADD FOREIGN KEY (station_id)
    REFERENCES petro_application.station (station_id) MATCH SIMPLE
    ON UPDATE CASCADE
    ON DELETE CASCADE
    NOT VALID;


ALTER TABLE IF EXISTS petro_application.dispenser
    ADD FOREIGN KEY (fuel_id)
    REFERENCES petro_application.fuel (fuel_id) MATCH SIMPLE
    ON UPDATE CASCADE
    ON DELETE SET NULL
    NOT VALID;


ALTER TABLE IF EXISTS petro_application.dispenser
    ADD FOREIGN KEY (tank_id)
    REFERENCES petro_application.tank (tank_id) MATCH SIMPLE
    ON UPDATE CASCADE
    ON DELETE SET NULL
    NOT VALID;


ALTER TABLE IF EXISTS petro_application.tank
    ADD FOREIGN KEY (fuel_id)
    REFERENCES petro_application.fuel (fuel_id) MATCH SIMPLE
    ON UPDATE CASCADE
    ON DELETE SET NULL
    NOT VALID;


ALTER TABLE IF EXISTS petro_application.tank
    ADD FOREIGN KEY (station_id)
    REFERENCES petro_application.station (station_id) MATCH SIMPLE
    ON UPDATE CASCADE
    ON DELETE CASCADE
    NOT VALID;


ALTER TABLE IF EXISTS petro_application.log
    ADD FOREIGN KEY (dispenser_id)
    REFERENCES petro_application.dispenser (dispenser_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION
    NOT VALID;

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
('Mipec Station 10', '987 Ba Hom, Binh Tri Dong Ward, Binh Tan District, Ho Chi Minh City'),
('Delete station', 'example address'),
('Update station', 'example address');

INSERT INTO petro_application.fuel (short_name, long_name, price) VALUES
('A95', 'RON 95-I', 25000),
('E5', 'E5 RON 92-II', 23000),
('DO1', 'Diesel Oil-I', 20000),
('DO5', 'Diesel Oil-V', 19000);

INSERT INTO petro_application.tank (fuel_id, station_id, name, max_volume) VALUES
(1, 1, 101, 5000), (2, 1, 102, 4000), (3, 1, 103, 6000), (4, 1, 104, 5500), (1, 1, 105, 7000),
(2, 2, 201, 5000), (3, 2, 202, 7500), (4, 2, 203, 6800), (1, 2, 204, 6000), (2, 2, 205, 4500),
(3, 3, 301, 7000), (4, 3, 302, 7200), (1, 3, 303, 8000), (2, 3, 304, 5500), (3, 3, 305, 6200),
(4, 4, 401, 7700), (1, 4, 402, 5200), (2, 4, 403, 4100), (3, 4, 404, 6300), (4, 4, 405, 5600),
(4, 11, 401, 7700), (1, 11, 402, 5200), (2, 11, 403, 4100), (3, 11, 404, 6300), (4, 11, 405, 5600),
(4, 12, 401, 7700), (1, 12, 402, 5200), (2, 12, 403, 4100), (3, 12, 404, 6300), (4, 12, 405, 5600);

INSERT INTO petro_application.dispenser (station_id, tank_id, fuel_id, name) VALUES
(1, 1, 1, 101), (1, 2, 2, 102), (1, 3, 3, 103), (1, 4, 4, 104), (1, 5, 1, 105),
(2, 6, 2, 201), (2, 7, 3, 202), (2, 8, 4, 203), (2, 9, 1, 204), (2, 10, 2, 205),
(3, 11, 3, 301), (3, 12, 4, 302), (3, 13, 1, 303), (3, 14, 2, 304), (3, 15, 3, 305),
(4, 16, 4, 401), (4, 17, 1, 402), (4, 18, 2, 403), (4, 19, 3, 404), (4, 20, 4, 405),
(11, 21, 4, 401), (11, 22, 1, 402), (11, 23, 2, 403), (11, 24, 3, 404), (11, 25, 4, 405),
(12, 26, 4, 401), (12, 27, 1, 402), (12, 28, 2, 403), (12, 29, 3, 404), (12, 30, 4, 405);

INSERT INTO petro_application.log (dispenser_id, fuel_name, log_type, total_liters, total_amount, time) VALUES
(1, 'A95', 1, 10.5, 26250, TIMESTAMP(0) '2025-07-04 08:30:00'),
(1, 'A95', 2, 8.2, 20500, TIMESTAMP(0) '2025-07-04 08:45:00'),
(1, 'A95', 2, 10.5, 26250, TIMESTAMP(0) '2025-07-04 14:10:00'),
(1, 'A95', 2, 10.5, 26250, TIMESTAMP(0) '2025-07-04 14:12:00'),
(1, 'A95', 2, 8.2, 20500, TIMESTAMP(0) '2025-07-04 15:30:00'),
(1, 'A95', 3, 12.0, 30000, TIMESTAMP(0) '2025-07-04 15:00:00'),
(2, 'E5', 2, 9.0, 22500, TIMESTAMP(0) '2025-07-04 16:45:00'),
(2, 'E5', 1, 9.5, 21850, TIMESTAMP(0) '2025-07-04 08:35:00'),
(2, 'E5', 2, 7.8, 17940, TIMESTAMP(0) '2025-07-04 08:50:00'),
(2, 'E5', 4, 11.2, 25760, TIMESTAMP(0) '2025-07-04 15:05:00'),
(2, 'E5', 3, 9.0, 22500, TIMESTAMP(0) '2025-07-04 16:45:00'),
(3, 'DO1', 1, 15.0, 30000, TIMESTAMP(0) '2025-07-04 08:40:00'),
(3, 'DO1', 1, 13.2, 26400, TIMESTAMP(0) '2025-07-04 08:55:00'),
(3, 'DO1', 1, 16.5, 33000, TIMESTAMP(0) '2025-07-04 08:10:00'),
(3, 'DO1', 3, 15.0, 30000, TIMESTAMP(0) '2025-07-04 17:20:00'),
(4, 'DO5', 4, 12.3, 23370, TIMESTAMP(0) '2025-07-04 08:45:00'),
(4, 'DO5', 4, 10.8, 20520, TIMESTAMP(0) '2025-07-04 08:00:00'),
(4, 'DO5', 4, 14.5, 27550, TIMESTAMP(0) '2025-07-04 08:15:00'),
(5, 'A95', 1, 10.5, 26250, TIMESTAMP(0) '2025-07-04 14:10:00'),
(5, 'A95', 2, 10.5, 26250, TIMESTAMP(0) '2025-07-04 14:10:00'),
(5, 'A95', 2, 10.5, 26250, TIMESTAMP(0) '2025-07-04 14:10:00'),
(5, 'A95', 3, 8.2, 20500, TIMESTAMP(0) '2025-07-04 15:30:00'),

(6, 'A95', 2, 11.0, 27500, TIMESTAMP(0) '2025-07-04 19:15:00'),
(6, 'A95', 2, 13.0, 29000, TIMESTAMP(0) '2025-07-04 20:05:00'),
(6, 'A95', 3, 16.2, 32000, TIMESTAMP(0) '2025-07-04 20:45:00'),
(6, 'A95', 4, 10.5, 26250, TIMESTAMP(0) '2025-07-04 14:10:00'),
(6, 'A95', 4, 8.2, 20500, TIMESTAMP(0) '2025-07-04 15:30:00'),
(7, 'E5', 1, 12.5, 28000, TIMESTAMP(0) '2025-07-04 18:50:00'),
(7, 'E5', 1, 9.0, 22500, TIMESTAMP(0) '2025-07-04 16:45:00'),
(7, 'E5', 1, 14.0, 31000, TIMESTAMP(0) '2025-07-04 21:30:00'),
(7, 'E5', 1, 9.5, 23000, TIMESTAMP(0) '2025-07-04 16:30:00'),
(7, 'E5', 1, 14.0, 31000, TIMESTAMP(0) '2025-07-04 21:30:00'),
(8, 'DO1', 2, 13.0, 29000, TIMESTAMP(0) '2025-07-04 20:05:00'),
(8, 'DO1', 2, 16.2, 32000, TIMESTAMP(0) '2025-07-04 20:45:00'),
(8, 'DO1', 2, 15.0, 30000, TIMESTAMP(0) '2025-07-04 17:20:00'),
(9, 'D05', 2, 10.5, 26250, now()),
(9, 'D05', 2, 8.2, 20500,now()),
(9, 'D05', 2, 11.0, 27500, now()),
(9, 'D05', 4, 9.8, 24800, TIMESTAMP(0) '2025-07-04 21:55:00'),
(9, 'D05', 4, 11.2, 28000, TIMESTAMP(0) '2025-07-04 11:15:00'),
(11, 'A95', 1, 11.0, 27500, TIMESTAMP(0) '2025-07-04 19:15:00'),
(11, 'A95', 2, 13.0, 29000, TIMESTAMP(0) '2025-07-04 08:05:00'),
(11, 'A95', 3, 16.2, 32000, TIMESTAMP(0) '2025-07-04 20:45:00'),
(11, 'A95', 4, 10.5, 26250, TIMESTAMP(0) '2025-07-04 11:10:00'),
(11, 'A95', 4, 8.2, 20500, TIMESTAMP(0) '2025-07-04 15:30:00'),
(12, 'E5', 2, 9.0, 22500, TIMESTAMP(0) '2025-07-04 05:45:00'),
(12, 'E5', 1, 12.5, 28000, TIMESTAMP(0) '2025-07-04 05:50:00'),
(12, 'E5', 4, 14.0, 31000, TIMESTAMP(0) '2025-07-04 05:30:00'),
(12, 'E5', 3, 9.5, 23000, TIMESTAMP(0) '2025-07-04 04:30:00'),
(13, 'DO1', 3, 13.5, 31000, TIMESTAMP(0) '2025-07-04 21:45:00'),
(13, 'DO1', 3, 15.0, 30000, TIMESTAMP(0) '2025-07-04 17:20:00'),
(13, 'DO1',2, 13.0, 29000, TIMESTAMP(0) '2025-07-04 20:05:00'),
(14, 'DO1', 4, 16.2, 32000, TIMESTAMP(0) '2025-07-04 20:45:00');

insert into petro_application.shift (shift_type, start_time, end_time) 
VALUES 
(1, '06:00:00', '14:00:00'),
(2, '14:00:00', '22:00:00'),
(3, '22:00:00', '06:00:00');

insert into petro_application.staff (staff_name, date_birth, phone, address, email) 
values 
('Nguyễn Yến Linh', '2004-01-01', '0331231588', '102 Nguyễn Quý Anh', 'yenlinh@gmail.com'), 
('Nguyễn Văn An', '1990-05-15', '0901234567', '123 Lê Duẩn, Quận 1, TP.HCM', 'annguyen@gmail.com'),
('Trần Thị Bích', '1988-09-20', '0912345678', '456 Nguyễn Trãi, Quận 5, TP.HCM', 'bichtran@gmail.com'),
('Lê Hoàng Nam', '1995-12-10', '0923456789', '789 Cách Mạng Tháng 8, Quận 3, TP.HCM', 'namle@gmail.com'),
('Phạm Minh Châu', '1992-07-25', '0934567890', '12 Hai Bà Trưng, Quận 1, TP.HCM', 'chaupham@gmail.com'),
('Võ Thanh Tùng', '1985-03-05', '0945678901', '345 Phan Đình Phùng, Phú Nhuận, TP.HCM', 'tungvo@gmail.com');

insert into petro_application.assignment (shift_id, staff_id, station_id, work_date) values
(1, 1, 1, '2015-07-02'),
(2, 3, 1, '2025-07-05'),
(3, 2, 1, '2025-07-05'),
(1, 3, 1, '2025-07-05'),
(2, 3, 1, '2025-07-06'),
(3, 5, 1, '2025-07-06'),
(3, 5, 1, '2025-07-06'),
(3, 5, 2, '2025-07-06'),
(3, 5, 1, '2025-07-07'),
(3, 5, 1, '2025-07-07'),
(3, 5, 1, '2025-07-07'),
(3, 5, 1, '2025-07-07'),
(3, 5, 1, '2025-07-07'),
(3, 5, 2, '2025-07-07');

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


