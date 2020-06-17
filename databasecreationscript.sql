#DATABASE
#DROP DATABASE IF EXISTS PlayTogether;
CREATE DATABASE IF NOT EXISTS PlayTogether;

#TABLES
DROP TABLE IF EXISTS Participants;
DROP TABLE IF EXISTS Games;
DROP TABLE IF EXISTS Users;
DROP TABLE IF EXISTS Roles;
DROP TABLE IF EXISTS Players;
DROP TABLE IF EXISTS Sport_types;
DROP TABLE IF EXISTS Places;
DROP TABLE IF EXISTS Surfaces;
DROP TABLE IF EXISTS Cities;
DROP TABLE IF EXISTS UsersLog;
DROP TABLE IF EXISTS PlayersLog;
DROP TABLE IF EXISTS GamesLog;
DROP TABLE IF EXISTS ParticipantsLog;

CREATE TABLE IF NOT EXISTS Roles(
role_id tinyint auto_increment not null comment "Klucz główny" primary key,
role_name varchar(10) unique not null
);
INSERT INTO Roles(role_name)
VALUES('admin'),
('moderator'),
('user');

CREATE TABLE IF NOT EXISTS Players(
player_id int auto_increment not null comment "Klucz główny" primary key,
first_name varchar(15) not null,
last_name varchar(25) not null,
nickname varchar(25) default null,
profile_picture varchar(256) default null,
birth_date date not null,
bio varchar(256) default null,
points_of_trust int default 0 not null,
games_attended int default 0 not null,
modified datetime default current_timestamp on update current_timestamp not null
);
INSERT INTO Players(first_name, last_name, nickname, birth_date)
VALUES ('Tomek', 'Florczuk', 'tomi.flox', '1997-01-22');

CREATE TABLE IF NOT EXISTS Users(
user_id int auto_increment not null comment "Klucz główny" primary key,
role_id tinyint default 3 not null comment "Klucz obcy, 1-admin, 2-moderator, 3-user",
login varchar(20) unique not null,
password varchar(256) not null,
email varchar(30) unique not null check (email LIKE '%@%'),
created datetime default current_timestamp not null comment "Czas utworzenia konta",
lastlogin datetime default null comment "Czas ostatniego zalogowania",
modified datetime default current_timestamp on update current_timestamp not null,
user_status char(1) default 'A' not null  comment "A - aktywny, B - zablokowany, D-usunięty" check (user_status IN ('A', 'B', 'D')),
player_id integer not null comment "Klucz obcy- powiązany z uzytkownikiem zawodnik",
constraint fk_user_role foreign key (role_id) references Roles(role_id) on update cascade,
constraint fk_user_player foreign key (player_id) references Players(player_id) on update cascade on delete restrict
);
ALTER TABLE Users auto_increment = 1;
INSERT INTO Users(login, password, email, player_id)
VALUES ('tomekf', 'haslo', 'tomek.florczuk@gmail.com', 1);

CREATE TABLE IF NOT EXISTS Sport_types(
type_id tinyint auto_increment not null comment "Klucz główny" primary key,
sport_type varchar(12) unique not null
);
INSERT INTO Sport_types(sport_type)
VALUES ('Piłka nożna'),
('Koszykówka'),
('Siatkówka');

CREATE TABLE IF NOT EXISTS Surfaces(
surface_id tinyint auto_increment not null comment "Klucz główny" primary key,
surface_name varchar(15) unique not null
);
INSERT INTO Surfaces(surface_name)
VALUES ('Tartan'),
('Naturalna trawa'),
('Sztuczna trawa'),
('Piasek'),
('Beton');

CREATE TABLE IF NOT EXISTS Cities(
city_id tinyint auto_increment not null comment 'Klucz główny' primary key,
city_name varchar(15) unique not null
);
INSERT INTO Cities(city_name)
VALUES ('Wrocław'),
('Katowice');

CREATE TABLE IF NOT EXISTS Places(
place_id int auto_increment not null comment "Klucz główny" primary key,
place_name varchar(25) unique not null comment "Nazwa/adres boiska",
city_id tinyint not null comment "Klucz obcy - miasto",
surface_id tinyint not null comment "Klucz obcy - typ nawierchni",
constraint fk_places_cities foreign key (city_id) references Cities(city_id),
constraint fk_places_surfaces foreign key (surface_id) references Surfaces(surface_id)
);

CREATE TABLE IF NOT EXISTS Games(
game_id int auto_increment not null comment "Klucz główny" primary key,
host_user int not null comment "Klucz obcy - Założyciel wydarzenia",
game_date datetime not null comment "Data wydarzenia",
game_length time not null comment "Długość trwania wydarzenia",
game_type tinyint not null comment "Klucz obcy - typ wydarzenia, 1 - piłka nożna, 2 - koszykówka, 3 - siatkówka",
max_players int not null comment "Maksmalna liczba uczestników" check (max_players > 0),
price tinyint default null comment "Cena od jednego uczestnika" check (price >= 0),
place_id int not null comment "Klucz obcy - miejsce wydarzenia",
created datetime default current_timestamp not null comment "Czas utworzenia wydarzenia",
game_status char(1) default 'A' not null comment "A - aktywne, B - odwołane, D - zakończone",
notes varchar(256) default null comment "Opis wydarzenia",
modified datetime default current_timestamp on update current_timestamp not null comment "Czas modyfikacji wydarzenia",
constraint fk_games_users foreign key (host_user) references Users(user_id) on update cascade,
constraint fk_games_places foreign key (place_id) references Places(place_id) on update cascade,
constraint fk_games_types foreign key (game_type) references Sport_types(type_id) on update cascade on delete restrict
);

CREATE TABLE IF NOT EXISTS Participants(
participant_id int auto_increment not null comment "Klucz główny" primary key,
player_id int not null comment "Klucz obcy - Zawodnik",
game_id int not null comment "Klucz obcy - Wydarzenie",
participant_status char(1) default 'S' not null comment "S - zapisany, U - wypisany" check (participant_status IN ('S', 'U')),
added datetime default current_timestamp not null comment "Czas zapisania się do wydarzenia",
modified datetime default current_timestamp on update current_timestamp not null comment "Czas modyfikacji wydarzenia",
constraint fk_partcipant_player foreign key (player_id) references Players(player_id),
constraint fk_participant_game foreign key (game_id) references Games(game_id)
);

CREATE TABLE IF NOT EXISTS UsersLog(
modification_id int auto_increment not null comment "Primary key" primary key,
modifier varchar(25) not null comment "Użytkownik wprowadzający zmianę",
modification_type char(1) not null comment "U-update, I-insert, D-delete",
old_user_id int,
new_user_id int,
old_login varchar(20),
new_login varchar(20),
old_role_id tinyint references Roles(role_id),
new_role_id tinyint references Roles(role_id),
old_password varchar(25),
new_password varchar(25),
old_email varchar(30),
new_email varchar(30),
old_user_status char(1),
new_user_status char(1),
old_player_id int references Players(player_id),
new_player_id int references Players(player_id),
modification_time datetime default current_timestamp not null
);

CREATE TABLE IF NOT EXISTS PlayersLog(
modification_id int auto_increment not null comment "Primary key" primary key,
modifier varchar(25) not null comment "Użytkownik wprowadzający zmianę",
modification_type char not null comment "U-update, I-insert, D-delete",
new_player_id int references Players(player_id),
old_player_id int references Players(player_id),
new_first_name varchar(15),
old_first_name varchar(15),
new_last_name varchar(25),
old_last_name varchar(25),
new_nickname varchar(25),
old_nickname varchar(25),
new_profile_picture varchar(256),
old_profile_picture varchar(256),
new_bio varchar(256),
old_bio varchar(256),
new_point_of_trust int,
old_points_of_trust int,
new_games_attended int,
old_games_attended int,
modification_time datetime default current_timestamp not null
);

CREATE TABLE IF NOT EXISTS GamesLog(
modification_id int auto_increment not null comment "Primary key" primary key,
modifier varchar(25) not null comment "Użytkownik wprowadzający zmianę",
modification_type char(1) not null comment "U-update, I-insert, D-delete",
old_game_id int,
new_game_id int,
old_host_user int references Users(user_id),
new_host_user int references Users(user_id),
old_game_date datetime,
new_game_date datetime,
old_game_length time,
new_game_length time,
old_game_type tinyint references Sport_types(type_id),
new_game_type tinyint references Sport_types(type_id),
old_max_players int,
new_max_players int,
old_price tinyint,
new_price tinyint,
old_place_id int references Places(place_id),
new_place_id int references Places(place_id),
old_game_status char(1),
new_game_status char(1),
old_notes varchar(256),
new_notes varchar(256),
modification_time datetime default current_timestamp not null
);

CREATE TABLE IF NOT EXISTS ParticipantsLog(
modification_id int auto_increment not null comment "Primary key" primary key,
modifier varchar(25) not null comment "Użytkownik wprowadzający zmianę",
modification_type char(1) not null comment "U-update, I-insert, D-delete",
old_participant_id int,
new_particiapnt_id int,
old_user_id int,
new_user_id int,
old_game_id int,
new_game_id int,
old_participant_status char(1),
new_participant_status char(1),
modification_time datetime default current_timestamp not null
);

#VIEWS
DROP VIEW IF EXISTS ListCities;
CREATE VIEW ListCities AS 
	select c.city_id, c.city_name 
	from Cities c 
	order by c.city_id;
	
DROP VIEW IF EXISTS ListSurfaces;
CREATE VIEW ListSurfaces AS 
	select s.surface_id, s.surface_name
	from Surfaces s 
	order by s.surface_id;

#PROCEDURES
DROP PROCEDURE IF EXISTS CheckEmailDuplicate;
CREATE PROCEDURE CheckEmailDuplicate(inmail varchar(30))
	SELECT user_id, login, password, email
	FROM Users
	WHERE email = inmail;

DROP PROCEDURE IF EXISTS CheckLoginDuplicate;
CREATE PROCEDURE CheckLoginDuplicate(inlogin varchar(20))
	SELECT user_id, login, password, email
    FROM Users
    WHERE login = inlogin;
	
DROP PROCEDURE IF EXISTS Logging;
CREATE PROCEDURE Logging(IN inlogin varchar(20), IN inpassword varchar(256), IN inmail varchar(30))
	SELECT user_id, login, password, email
    FROM Users
    WHERE (login = inlogin OR email = inmail)
    AND password = inpassword;

DROP PROCEDURE IF EXISTS UpcomingGames;
DELIMITER $$
CREATE PROCEDURE UpcomingGames(insportype int, incityid int)
BEGIN
	IF insporttype = 0 THEN
		SELECT * FROM  Games g JOIN Places p USING (place_id)
        WHERE p.city_id = incityid
        AND g.game_date > NOW()
		AND g.game_status = 'A'
        ORDER BY g.game_date ASC;
	ELSE
		SELECT * FROM  Games g JOIN Places p USING (place_id)
        WHERE p.city_id = incityid
        AND g.game_type = insporttype
        AND g.game_date > NOW()
		AND g.game_status = 'A'
        ORDER BY g.game_date ASC;
	END IF;
END$$
DELIMITER ;

DROP PROCEDURE IF EXISTS CountPlayersInGame;
CREATE PROCEDURE CountPlayersInGame(ingameid int)
	SELECT COUNT(*) AS CurrentPlayersAmount
    FROM Participants p
    WHERE p.game_id = ingameid
    GROUP BY p.game_id;

DROP PROCEDURE IF EXISTS ListParticipantsInGame;
CREATE PROCEDURE ListParticipantsInGame(gameid int)
	SELECT * FROM Participants p
    WHERE p.games_id = gameid
    AND p.participant_status = 'A';
	
DROP procedure IF EXISTS ListPlaces;
DELIMITER $$
CREATE PROCEDURE ListPlaces(incityid int)
BEGIN
	IF incityid != 0  THEN 
		SELECT *
        FROM Places p
        WHERE p.city_id = incityid;
	END IF;
END$$

DELIMITER ;
#TRIGGERS

#Users triggers
DROP TRIGGER IF EXISTS UpdateOnUsers;
CREATE TRIGGER UpdateOnUsers BEFORE UPDATE ON Users
FOR EACH ROW
INSERT INTO UsersLog
VALUES (current_user, 'U', OLD.user_id, NEW.user_id, OLD.login, NEW.login, OLD.role_id, NEW.role_id, OLD.password, NEW.password,
	OLD.email, NEW.email, OLD.user_status, NEW.user_status, OLD.player_id, NEW.player_id, current_timestamp);

DROP TRIGGER IF EXISTS InsertOnUsers;
CREATE TRIGGER InsertOnUsers AFTER INSERT ON Users
FOR EACH ROW
INSERT INTO UsersLog (modifier, modification_type, new_user_id, new_login, new_role_id, new_password, new_email, new_user_status, new_player_id, modification_time) 
VALUES (current_user, 'I', NEW.user_id, NEW.login, NEW.role_id, NEW.password, NEW.email, NEW.user_status, NEW.player_id, current_timestamp);

DROP TRIGGER IF EXISTS DeleteOnUsers;
CREATE TRIGGER DeleteOnUsers AFTER DELETE ON Users
FOR EACH ROW
INSERT INTO UsersLog (modifier, modification_type, old_user_id, old_login, old_role_id, old_password, old_email, old_user_status, old_player_id, modification_time)
VALUES (current_user, 'D', OLD.user_id, OLD.login, OLD.role_id, OLD.password, OLD.email, OLD.user_status, OLD.player_id, current_timestamp);

DROP TRIGGER IF EXISTS UpdateOnGames;
CREATE TRIGGER UpdateOnGames BEFORE UPDATE ON Games
FOR EACH ROW
INSERT INTO GamesLog
VALUES (current_user, 'U', OLD.game_id, NEW.game_id, OLD.host_user, NEW.host_user, OLD.game_date, NEW.game_date,
    OLD.game_length, NEW.game_length, OLD.game_type, NEW.game_type, OLD.max_players, NEW.max_players, OLD.price, NEW.price,
    OLD.place_id, NEW.place_id, OLD.game_status, NEW.game_status, OLD.notes, NEW.notes, current_timestamp);

#Players triggers
DROP TRIGGER IF EXISTS DeleteOnPlayers;
CREATE TRIGGER DeleteOnPlayers AFTER DELETE ON Players
FOR EACH ROW
INSERT INTO PlayersLog(modifier, modification_type, old_player_id, old_first_name, old_last_name, 
	old_nickname, old_picture, old_bio, old_points_of_trust, old_games_attended, modification_time)
VALUES(current_user, 'D', OLD.player_id, OLD.first_name, OLD.last_name, OLD.nickname, OLD.profile_picture, OLD.bio,
	OLD.points_of_trust, OLD.games_attended, current_timestamp);

DROP TRIGGER IF EXISTS InsertOnPlayers;
CREATE TRIGGER InsertOnPlayers AFTER INSERT ON Players
FOR EACH ROW
INSERT INTO PlayersLog(modifier, modification_type, new_player_id, new_first_name, new_last_name, 
    new_nickname, new_picture, new_bio, new_points_of_trust, new_games_attended, modification_time)
VALUES(current_user, 'I', NEW.player_id, NEW.first_name, NEW.last_name, NEW.nickname, NEW.profile_picture, NEW.bio,
    NEW.points_of_trust, NEW.games_attended, current_timestamp);

DROP TRIGGER IF EXISTS UpdateOnPlayers;
CREATE TRIGGER UpdateOnPlayers BEFORE UPDATE ON Players
FOR EACH ROW
INSERT INTO PlayersLog
VALUES (current_user, 'I', NEW.player_id, OLD.player_id, NEW.first_name, OLD.first_name, NEW.last_name, OLD.last_name, NEW.nickname, OLD.nickname, NEW.profile_picture, OLD.profile_picture, NEW.bio, OLD.bio,
	NEW.points_of_trust, OLD.points_of_trust, NEW.games_attended, OLD.games_attended, current_timestamp);

#Games triggers
DROP TRIGGER IF EXISTS InsertOnGames;
CREATE TRIGGER InsertOnGames AFTER INSERT ON Games
FOR EACH ROW
INSERT INTO GamesLog(modifier, modification_type, new_game_id, new_host_user, new_game_date, new_game_length,
    new_game_type, new_max_players, new_price, new_game_status, new_notes)
VALUES (current_user, 'I', NEW.game_id, NEW.host_user, NEW.game_date, NEW.game_length, NEW.game_type, NEW.max_players,
    NEW.price, NEW.place_id, NEW.game_status, NEW.notes);

CREATE TRIGGER DeleteOnGames AFTER DELETE ON Games
FOR EACH ROW
INSERT INTO GamesLog(modifier, modification_type, old_game_id, old_host_user, old_game_date, old_game_length,
    old_game_type, old_max_players, old_price, old_game_status, old_notes)
VALUES (current_user, 'D', OLD.game_id, OLD.host_user, OLD.game_date, OLD.game_length, OLD.game_type, OLD.max_players,
    OLD.price, OLD.place_id, OLD.game_status, OLD.notes);

#Participants triggers
DROP TRIGGER IF EXISTS DeleteOnParticiapnts;
CREATE TRIGGER DeleteOnParticiapnts AFTER DELETE ON Participants
FOR EACH ROW
INSERT INTO ParticipantsLog(modifier, modification_type, old_particiapnt_id, old_player_id, old_game_id, old_participant_status, modification_time)
VALUES(current_user, 'D', OLD.participant_id, OLD.player_id, OLD.game_id, OLD.participant_status, current_timestamp);

DROP TRIGGER IF EXISTS InsertOnParticipants;
CREATE TRIGGER InsertOnParticipants AFTER INSERT ON Participants
FOR EACH ROW
INSERT INTO ParticipantsLog(modifier, modification_type, new_participant_id, new_player_id, new_game_id, new_participant_status, modification_time)
VALUES(current_user, 'I', NEW.participant_id, NEW.player_id, NEW.game_id, NEW.participant_status, current_timestamp);

DROP TRIGGER IF EXISTS UpdateOnParticipants;
CREATE TRIGGER UpdateOnParticipants BEFORE UPDATE ON Participants
FOR EACH ROW
INSERT INTO ParticipantsLog
VALUES(current_user, 'U', OLD.participant_id, NEW.participant_id, OLD.player_id, NEW.player_id, OLD.game_id, NEW.game_id, OLD.participant_status, NEW.participant_status, current_timestamp);

DROP TRIGGER IF EXISTS AddHostToParticipants;
CREATE TRIGGER AddHostToParticipants AFTER INSERT ON Games
FOR EACH ROW
INSERT INTO Participants(user_id, game_id)
VALUES (NEW.host_user, NEW.game_id);