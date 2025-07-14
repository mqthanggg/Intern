BEGIN;

SET search_path TO petro_application, public;

ALTER TABLE IF EXISTS petro_application.assignment DROP CONSTRAINT IF EXISTS assignment_shift_id_fkey;

ALTER TABLE IF EXISTS petro_application.assignment DROP CONSTRAINT IF EXISTS assignment_staff_id_fkey;

ALTER TABLE IF EXISTS petro_application.assignment DROP CONSTRAINT IF EXISTS assignment_station_id_fkey;

ALTER TABLE IF EXISTS petro_application.detailreceipt DROP CONSTRAINT IF EXISTS detailreceipt_receipt_id_fkey ;

ALTER TABLE IF EXISTS petro_application.receipt DROP CONSTRAINT IF EXISTS receipt_supplier_id_fkey;

ALTER TABLE IF EXISTS petro_application.dispenser DROP CONSTRAINT IF EXISTS None;

ALTER TABLE IF EXISTS petro_application.dispenser DROP CONSTRAINT IF EXISTS None;

ALTER TABLE IF EXISTS petro_application.dispenser DROP CONSTRAINT IF EXISTS None;

ALTER TABLE IF EXISTS petro_application.tank DROP CONSTRAINT IF EXISTS None;

ALTER TABLE IF EXISTS petro_application.tank DROP CONSTRAINT IF EXISTS None;

ALTER TABLE IF EXISTS petro_application.log DROP CONSTRAINT IF EXISTS None;

ALTER TABLE IF EXISTS petro_application.receipt DROP CONSTRAINT IF EXISTS None;

ALTER TABLE IF EXISTS petro_application.detailreceipt DROP CONSTRAINT IF EXISTS None;

ALTER TABLE IF EXISTS petro_application.supplier DROP CONSTRAINT IF EXISTS None;

ALTER TABLE IF EXISTS petro_application.dispenser DROP CONSTRAINT IF EXISTS None;

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
    role integer NOT NULL,
    CHECK (role in (1,2)),
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
    IS 'Table for storing user''s credentials. Role: 1 -> user, 2 -> administrator';

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

DROP TABLE IF EXISTS petro_application.supplier;

CREATE TABLE IF NOT EXISTS petro_application.supplier (
    supplier_id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 ),
    supplier_name character varying(30) NOT NULL,
    phone integer NOT NULL,
    address character varying(50) NOT NULL,
    email character varying(50) NOT NULL,
    created_by character varying(50),
    created_date time with time zone DEFAULT now(),
    last_modified_by character varying(50),
    last_modified_date time with time zone DEFAULT now(),
	 PRIMARY KEY (supplier_id)
);

COMMENT ON TABLE petro_application.supplier
    IS 'for supplier''s information';
	
DROP TABLE IF EXISTS petro_application.receipt;

CREATE TABLE IF NOT EXISTS petro_application.receipt (
    receipt_id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 ),
    receipt_date time with time zone NOT NULL,
    supplier_id integer NOT NULL,
    station_id integer NOT NULL,
    total_amount integer NOT NULL,
    created_by character varying(30),
    created_date time with time zone DEFAULT now(),
    last_modified_by character varying(30),
    last_modified_date time with time zone DEFAULT now(),
	PRIMARY KEY (receipt_id)
);

COMMENT ON TABLE petro_application.receipt
    IS 'for import receipt information';

DROP TABLE IF EXISTS petro_application.detailreceipt;

CREATE TABLE IF NOT EXISTS petro_application.detailreceipt (
    receipt_id integer NOT NULL,
    fuel_id integer NOT NULL,
    liters_fuel integer NOT NULL,
    price integer NOT NULL,
  	created_by character varying(30) DEFAULT now(),
    created_date timestamp(0) with time zone DEFAULT now(),
    last_modified_by character varying(30),
    last_modified_date timestamp(0) with time zone DEFAULT now(),
	PRIMARY KEY (receipt_id, fuel_id)
);

COMMENT ON TABLE petro_application.detailreceipt
    IS 'the import detail receipt information of each fuel';
	
DROP TABLE IF EXISTS petro_application.assignment;

CREATE TABLE IF NOT EXISTS petro_application.assignment
(
    assignment_id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 ),
    shift_id integer NOT NULL,
    staff_id integer NOT NULL,
    station_id integer NOT NULL ,
	work_date date NOT NULL, 
    created_by character varying(255),
    created_date timestamp(0) with time zone DEFAULT now(),
    last_modified_by character varying(255),
    last_modified_date timestamp(0) with time zone DEFAULT now(),
    CONSTRAINT assignment_pkey PRIMARY KEY (assignment_id)
);

COMMENT ON TABLE petro_application.assignment
    IS 'for staff '' work schedule in each station';

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

ALTER TABLE IF EXISTS petro_application.receipt
    ADD FOREIGN KEY (supplier_id)
    REFERENCES petro_application.supplier (supplier_id) MATCH SIMPLE
    ON UPDATE CASCADE
    ON DELETE CASCADE
    NOT VALID;

ALTER TABLE IF EXISTS petro_application.detailrecepit
    ADD FOREIGN KEY (station_id)
    REFERENCES petro_application.station (station_id) MATCH SIMPLE
    ON UPDATE CASCADE
    ON DELETE CASCADE
    NOT VALID;

ALTER TABLE IF EXISTS petro_application.detailreceipt
    ADD FOREIGN KEY (fuel_id) 
	REFERENCES petro_application.fuel(fuel_id) MATCH SIMPLE
    ON UPDATE CASCADE
    ON DELETE CASCADE
    NOT VALID;

INSERT INTO petro_application.station (name, address, created_by, last_modified_by) VALUES
('Petrolimex Station 1', '123 Nguyen Hue, Ben Nghe Ward, District 1, Ho Chi Minh City', 'admin', 'admin'),
('PV Oil Station 2', '456 Cach Mang Thang 8, Ward 5, District 3, Ho Chi Minh City', 'admin', 'admin'),
('Saigon Petro Station 3', '789 Tran Hung Dao, Ward 6, District 5, Ho Chi Minh City', 'admin', 'admin'),
('COMECO Station 4', '101 Le Van Luong, Tan Phong Ward, District 7, Ho Chi Minh City', 'admin', 'admin'),
('Mipec Station 5', '234 Vo Van Tan, Ward 11, District 10, Ho Chi Minh City', 'admin', 'admin'),
('Petrolimex Station 6', '567 Xa Lo Ha Noi, An Phu Ward, District 2, Ho Chi Minh City', 'admin', 'admin'),
('PV Oil Station 7', '890 Hoang Dieu, Ward 9, District 4, Ho Chi Minh City', 'admin', 'admin'),
('Saigon Petro Station 8', '321 Pham Van Dong, Linh Trung Ward, Thu Duc City, Ho Chi Minh City', 'admin', 'admin'),
('COMECO Station 9', '654 Phan Van Tri, Ward 11, Go Vap District, Ho Chi Minh City', 'admin', 'admin'),
('Mipec Station 10', '987 Ba Hom, Binh Tri Dong Ward, Binh Tan District, Ho Chi Minh City', 'admin', 'admin'),
('Delete station', 'example address', 'admin', 'admin'),
('Update station', 'example address', 'admin', 'admin');

INSERT INTO petro_application.fuel (short_name, long_name, price,created_by, last_modified_by) VALUES
('A95', 'RON 95-I', 25000, 'admin', 'admin'),
('E5', 'E5 RON 92-II', 23000, 'admin', 'admin'),
('DO1', 'Diesel Oil-I', 20000, 'admin', 'admin'),
('DO5', 'Diesel Oil-V', 19000, 'admin', 'admin');

INSERT INTO petro_application.tank (fuel_id, station_id, name, max_volume, created_by, last_modified_by) VALUES
(1, 1, 101, 5000, 'admin', 'admin'), (2, 1, 102, 4000, 'admin', 'admin'), (3, 1, 103, 6000, 'admin', 'admin'), (4, 1, 104, 5500, 'admin', 'admin'), (1, 1, 105, 7000, 'admin', 'admin'),
(2, 2, 201, 5000, 'admin', 'admin'), (3, 2, 202, 7500, 'admin', 'admin'), (4, 2, 203, 6800, 'admin', 'admin'), (1, 2, 204, 6000, 'admin', 'admin'), (2, 2, 205, 4500, 'admin', 'admin'),
(3, 3, 301, 7000, 'admin', 'admin'), (4, 3, 302, 7200, 'admin', 'admin'), (1, 3, 303, 8000, 'admin', 'admin'), (2, 3, 304, 5500, 'admin', 'admin'), (3, 3, 305, 6200, 'admin', 'admin'),
(4, 4, 401, 7700, 'admin', 'admin'), (1, 4, 402, 5200, 'admin', 'admin'), (2, 4, 403, 4100, 'admin', 'admin'), (3, 4, 404, 6300, 'admin', 'admin'), (4, 4, 405, 5600, 'admin', 'admin'),
(4, 11, 401, 7700, 'admin', 'admin'), (1, 11, 402, 5200, 'admin', 'admin'), (2, 11, 403, 4100, 'admin', 'admin'), (3, 11, 404, 6300, 'admin', 'admin'), (4, 11, 405, 5600, 'admin', 'admin'),
(4, 12, 401, 7700, 'admin', 'admin'), (1, 12, 402, 5200, 'admin', 'admin'), (2, 12, 403, 4100, 'admin', 'admin'), (3, 12, 404, 6300, 'admin', 'admin'), (4, 12, 405, 5600, 'admin', 'admin');

INSERT INTO petro_application.dispenser (station_id, tank_id, fuel_id, name, created_by, last_modified_by) VALUES
(1, 1, 1, 101, 'admin', 'admin'), (1, 2, 2, 102, 'admin', 'admin'), (1, 3, 3, 103, 'admin', 'admin'), (1, 4, 4, 104, 'admin', 'admin'), (1, 5, 1, 105, 'admin', 'admin'),
(2, 6, 2, 201, 'admin', 'admin'), (2, 7, 3, 202, 'admin', 'admin'), (2, 8, 4, 203, 'admin', 'admin'), (2, 9, 1, 204, 'admin', 'admin'), (2, 10, 2, 205, 'admin', 'admin'),
(3, 11, 3, 301, 'admin', 'admin'), (3, 12, 4, 302, 'admin', 'admin'), (3, 13, 1, 303, 'admin', 'admin'), (3, 14, 2, 304, 'admin', 'admin'), (3, 15, 3, 305, 'admin', 'admin'),
(4, 16, 4, 401, 'admin', 'admin'), (4, 17, 1, 402, 'admin', 'admin'), (4, 18, 2, 403, 'admin', 'admin'), (4, 19, 3, 404, 'admin', 'admin'), (4, 20, 4, 405, 'admin', 'admin'),
(11, 21, 4, 401, 'admin', 'admin'), (11, 22, 1, 402, 'admin', 'admin'), (11, 23, 2, 403, 'admin', 'admin'), (11, 24, 3, 404, 'admin', 'admin'), (11, 25, 4, 405, 'admin', 'admin'),
(12, 26, 4, 401, 'admin', 'admin'), (12, 27, 1, 402, 'admin', 'admin'), (12, 28, 2, 403, 'admin', 'admin'), (12, 29, 3, 404, 'admin', 'admin'), (12, 30, 4, 405, 'admin', 'admin');

INSERT INTO petro_application.log (dispenser_id, fuel_name, log_type, total_liters, total_amount, time, created_by, last_modified_by) VALUES
(3, 'DO1', 3, 12.0, 30000, TIMESTAMP(0) '2025-02-04 17:20:00', 'admin', 'admin'),
(1, 'A95', 1, 10.5, 26250, TIMESTAMP(0) '2025-07-04 08:30:00', 'admin', 'admin'),
(1, 'A95', 2, 8.2, 20500, TIMESTAMP(0) '2025-07-04 08:45:00', 'admin', 'admin'),
(1, 'A95', 2, 10.5, 26250, TIMESTAMP(0) '2025-07-04 14:10:00', 'admin', 'admin'),
(1, 'A95', 2, 10.5, 26250, TIMESTAMP(0) '2025-07-04 14:12:00', 'admin', 'admin'),
(1, 'A95', 2, 8.2, 20500, TIMESTAMP(0) '2025-07-04 15:30:00', 'admin', 'admin'),
(1, 'A95', 3, 12.0, 30000, TIMESTAMP(0) '2025-07-04 15:00:00', 'admin', 'admin'),
(2, 'E5', 2, 9.0, 22500, TIMESTAMP(0) '2025-07-04 16:45:00', 'admin', 'admin'),
(2, 'E5', 1, 9.5, 21850, TIMESTAMP(0) '2025-07-04 08:35:00', 'admin', 'admin'),
(2, 'E5', 2, 7.8, 17940, TIMESTAMP(0) '2025-07-04 08:50:00', 'admin', 'admin'),
(2, 'E5', 4, 11.2, 25760, TIMESTAMP(0) '2025-07-04 15:05:00', 'admin', 'admin'),
(2, 'E5', 3, 9.0, 22500, TIMESTAMP(0) '2025-07-04 16:45:00', 'admin', 'admin'),
(3, 'DO1', 1, 15.0, 30000, TIMESTAMP(0) '2025-07-04 08:40:00', 'admin', 'admin'),
(3, 'DO1', 1, 13.2, 26400, TIMESTAMP(0) '2025-07-04 08:55:00', 'admin', 'admin'),
(3, 'DO1', 1, 16.5, 33000, TIMESTAMP(0) '2025-07-04 08:10:00', 'admin', 'admin'),
(3, 'DO1', 3, 15.0, 30000, TIMESTAMP(0) '2025-07-04 17:20:00', 'admin', 'admin'),
(4, 'DO5', 4, 12.3, 23370, TIMESTAMP(0) '2025-07-04 08:45:00', 'admin', 'admin'),
(4, 'DO5', 4, 10.8, 20520, TIMESTAMP(0) '2025-07-04 08:00:00', 'admin', 'admin'),
(4, 'DO5', 4, 14.5, 27550, TIMESTAMP(0) '2025-07-04 08:15:00', 'admin', 'admin'),
(5, 'A95', 1, 10.5, 26250, TIMESTAMP(0) '2025-07-04 14:10:00', 'admin', 'admin'),
(5, 'A95', 2, 10.5, 26250, TIMESTAMP(0) '2025-07-04 14:10:00', 'admin', 'admin'),
(5, 'A95', 2, 10.5, 26250, TIMESTAMP(0) '2025-07-04 14:10:00', 'admin', 'admin'),
(5, 'A95', 3, 8.2, 20500, TIMESTAMP(0) '2025-07-04 15:30:00', 'admin', 'admin'),

(6, 'A95', 2, 11.0, 27500, TIMESTAMP(0) '2025-07-04 19:15:00', 'admin', 'admin'),
(6, 'A95', 2, 13.0, 29000, TIMESTAMP(0) '2025-07-04 20:05:00', 'admin', 'admin'),
(6, 'A95', 3, 16.2, 32000, TIMESTAMP(0) '2025-07-04 20:45:00', 'admin', 'admin'),
(6, 'A95', 4, 10.5, 26250, TIMESTAMP(0) '2025-07-04 14:10:00', 'admin', 'admin'),
(6, 'A95', 4, 8.2, 20500, TIMESTAMP(0) '2025-07-04 15:30:00', 'admin', 'admin'),
(7, 'E5', 1, 12.5, 28000, TIMESTAMP(0) '2025-07-04 18:50:00', 'admin', 'admin'),
(7, 'E5', 1, 9.0, 22500, TIMESTAMP(0) '2025-07-04 16:45:00', 'admin', 'admin'),
(7, 'E5', 1, 14.0, 31000, TIMESTAMP(0) '2025-07-04 21:30:00', 'admin', 'admin'),
(7, 'E5', 1, 9.5, 23000, TIMESTAMP(0) '2025-07-04 16:30:00', 'admin', 'admin'),
(7, 'E5', 1, 14.0, 31000, TIMESTAMP(0) '2025-07-04 21:30:00', 'admin', 'admin'),
(8, 'DO1', 2, 13.0, 29000, TIMESTAMP(0) '2025-07-04 20:05:00', 'admin', 'admin'),
(8, 'DO1', 2, 16.2, 32000, TIMESTAMP(0) '2025-07-04 20:45:00', 'admin', 'admin'),
(8, 'DO1', 2, 15.0, 30000, TIMESTAMP(0) '2025-07-04 17:20:00', 'admin', 'admin'),
(9, 'D05', 2, 10.5, 26250, now(),'admin', 'admin'),
(9, 'D05', 2, 8.2, 20500,now(),'admin', 'admin'),
(9, 'D05', 2, 11.0, 27500, now(),'admin', 'admin'),
(9, 'D05', 4, 9.8, 24800, TIMESTAMP(0) '2025-07-04 21:55:00', 'admin', 'admin'),
(9, 'D05', 4, 11.2, 28000, TIMESTAMP(0) '2025-07-04 11:15:00', 'admin', 'admin'),
(11, 'A95', 1, 11.0, 27500, TIMESTAMP(0) '2025-07-04 19:15:00', 'admin', 'admin'),
(11, 'A95', 2, 13.0, 29000, TIMESTAMP(0) '2025-07-04 08:05:00', 'admin', 'admin'),
(11, 'A95', 3, 16.2, 32000, TIMESTAMP(0) '2025-07-04 20:45:00', 'admin', 'admin'),
(11, 'A95', 4, 10.5, 26250, TIMESTAMP(0) '2025-07-04 11:10:00', 'admin', 'admin'),
(11, 'A95', 4, 8.2, 20500, TIMESTAMP(0) '2025-07-04 15:30:00', 'admin', 'admin'),
(12, 'E5', 2, 9.0, 22500, TIMESTAMP(0) '2025-07-04 05:45:00', 'admin', 'admin'),
(12, 'E5', 1, 12.5, 28000, TIMESTAMP(0) '2025-07-04 05:50:00', 'admin', 'admin'),
(12, 'E5', 4, 14.0, 31000, TIMESTAMP(0) '2025-07-04 05:30:00', 'admin', 'admin'),
(12, 'E5', 3, 9.5, 23000, TIMESTAMP(0) '2025-07-04 04:30:00', 'admin', 'admin'),
(13, 'DO1', 3, 13.5, 31000, TIMESTAMP(0) '2025-07-04 21:45:00', 'admin', 'admin'),
(13, 'DO1', 3, 15.0, 30000, TIMESTAMP(0) '2025-07-04 17:20:00', 'admin', 'admin'),
(13, 'DO1',2, 13.0, 29000, TIMESTAMP(0) '2025-07-04 20:05:00', 'admin', 'admin'),
(14, 'DO1', 4, 16.2, 32000, TIMESTAMP(0) '2025-07-04 20:45:00', 'admin', 'admin');

insert into petro_application.shift (shift_type, start_time, end_time, created_by, last_modified_by) 
VALUES 
(1, '06:00:00', '14:00:00','admin', 'admin'),
(2, '14:00:00', '22:00:00','admin', 'admin'),
(3, '22:00:00', '06:00:00','admin', 'admin');

insert into petro_application.staff (staff_name, date_birth, phone, address, email, created_by, last_modified_by) 
values 
('Nguyễn Yến Linh', '2004-01-01', '0331231588', '102 Nguyễn Quý Anh', 'yenlinh@gmail.com','admin', 'admin'), 
('Nguyễn Văn An', '1990-05-15', '0901234567', '123 Lê Duẩn, Quận 1, TP.HCM', 'annguyen@gmail.com','admin', 'admin'),
('Trần Thị Bích', '1988-09-20', '0912345678', '456 Nguyễn Trãi, Quận 5, TP.HCM', 'bichtran@gmail.com','admin', 'admin'),
('Lê Hoàng Nam', '1995-12-10', '0923456789', '789 Cách Mạng Tháng 8, Quận 3, TP.HCM', 'namle@gmail.com','admin', 'admin'),
('Phạm Minh Châu', '1992-07-25', '0934567890', '12 Hai Bà Trưng, Quận 1, TP.HCM', 'chaupham@gmail.com','admin', 'admin'),
('Võ Thanh Tùng', '1985-03-05', '0945678901', '345 Phan Đình Phùng, Phú Nhuận, TP.HCM', 'tungvo@gmail.com','admin', 'admin');

insert into petro_application.assignment (shift_id, staff_id, station_id, work_date, created_by, last_modified_by) values
(1, 1, 1, '2015-07-02', 'admin', 'admin'),
(2, 3, 1, '2025-07-05', 'admin', 'admin'),
(3, 2, 1, '2025-07-05', 'admin', 'admin'),
(1, 3, 1, '2025-07-05', 'admin', 'admin'),
(2, 3, 1, '2025-07-06', 'admin', 'admin'),
(3, 5, 1, '2025-07-06', 'admin', 'admin'),
(3, 5, 1, '2025-07-06', 'admin', 'admin'),
(3, 5, 2, '2025-07-06', 'admin', 'admin'),
(3, 5, 1, '2025-07-07', 'admin', 'admin'),
(3, 5, 1, '2025-07-07', 'admin', 'admin'),
(3, 5, 1, '2025-07-07', 'admin', 'admin'),
(3, 5, 1, '2025-07-07', 'admin', 'admin'),
(3, 5, 1, '2025-07-07', 'admin', 'admin'),
(3, 5, 2, '2025-07-07', 'admin', 'admin');

INSERT INTO petro_application.supplier (supplier_name, phone, address, email, created_by, last_modified_by) VALUES
('Petrolimex Sài Gòn', 0283822888, '15 Lê Duẩn, Quận 1, TP.HCM', 'contact@petrolimex.com.vn','admin', 'admin'),
('PV Oil Miền Nam', 0283829898, '35 Nguyễn Huệ, Quận 1, TP.HCM', 'info@pvoil.com.vn','admin', 'admin'),
('Saigon Petro', 0283827755, '60 Trương Định, Quận 3, TP.HCM', 'sales@saigonpetro.com.vn', 'admin', 'admin'),
('Nam Sông Hậu Petro', 0292355550, 'Số 2 Trần Hưng Đạo, Ninh Kiều, Cần Thơ', 'cskh@namsonghau.vn','admin', 'admin'),
('Petro Bình Minh', 0283844660, '105 Lý Thường Kiệt, Tân Bình, TP.HCM', 'support@binhminhpetro.vn','admin', 'admin');

INSERT INTO petro_application.receipt (receipt_date, supplier_id, station_id, total_amount, created_by, last_modified_by)
VALUES
('2024-07-11 08:30:00-07', 1, 1, 50000000, 'admin', 'admin'),
('2024-07-11 09:00:00-07', 2, 2, 75000000, 'admin', 'admin'),
('2024-07-11 10:15:00-07', 3, 1, 62000000, 'admin', 'admin'),
('2024-07-11 11:45:00-07', 4, 3, 81000000, 'admin', 'admin'),
('2024-07-11 13:00:00-07', 5, 4, 55000000, 'admin', 'admin'),
('2024-07-11 14:20:00-07', 1, 5, 70000000, 'admin', 'admin'),
('2024-07-11 15:35:00-07', 2, 2, 46000000, 'admin', 'admin'),
('2024-07-11 16:50:00-07', 3, 6, 88000000, 'admin', 'admin'),
('2024-07-11 18:10:00-07', 4, 3, 93000000, 'admin', 'admin'),
('2024-07-11 19:25:00-07', 5, 1, 50000000, 'admin', 'admin');

INSERT INTO petro_application.detailreceipt (receipt_id, fuel_id, liters_fuel, price, created_by, last_modified_by)
VALUES
(1, 1, 5000, 20000, 'admin', 'admin'),
(1, 2, 3000, 18000, 'admin', 'admin'),
(2, 1, 4500, 21000, 'admin', 'admin'),
(2, 3, 2000, 22000, 'admin', 'admin'),
(3, 1, 6000, 20500, 'admin', 'admin'),
(4, 2, 3500, 19000, 'admin', 'admin'),
(5, 1, 4800, 20000, 'admin', 'admin'),
(6, 4, 2500, 23000, 'admin', 'admin'),
(7, 3, 3000, 21500, 'admin', 'admin'),
(8, 2, 4000, 18500, 'admin', 'admin');

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



