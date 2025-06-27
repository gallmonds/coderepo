CREATE TABLE IF NOT EXISTS content_type (
	type_id INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL,
	type_name VARCHAR(255) NOT NULL,
	created_at TIMESTAMP DEFAULT NOW() NOT NULL,
	audit_isdeleted CHAR(1) DEFAULT 0 NOT NULL,
	version INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS reportstatus (

	status_id INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL,
	status_name VARCHAR(255) NOT NULL,
	created_at TIMESTAMP DEFAULT NOW() NOT NULL,
	audit_isdeleted CHAR(1) DEFAULT 0 NOT NULL,
	version INTEGER NOT NULL

);

CREATE TABLE IF NOT EXISTS category (


	category_id INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL,
	category_name VARCHAR(255) NOT NULL,

	created_at TIMESTAMP DEFAULT NOW() NOT NULL,
	audit_isdeleted CHAR(1) DEFAULT 0 NOT NULL,
	version INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS media (

	media_id INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL,
	file_path VARCHAR(255) NOT NULL,
	mime_type VARCHAR(50) NOT NULL,
	category_id INT REFERENCES category(category_id) NOT NULL,
	created_at TIMESTAMP DEFAULT NOW() NOT NULL,
	audit_isdeleted CHAR(1) DEFAULT 0 NOT NULL,
	version INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS permission (
	permission_id INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL,
	permission_name VARCHAR(255) NOT NULL,
	created_at TIMESTAMP DEFAULT NOW() NOT NULL,
	audit_isdeleted CHAR(1) DEFAULT 0 NOT NULL,
	version INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS tag (
	tag_id INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL,
	tag_name VARCHAR(255) NOT NULL,
	created_at TIMESTAMP DEFAULT NOW() NOT NULL,
	audit_isdeleted CHAR(1) DEFAULT 0 NOT NULL,
	version INTEGER NOT NULL
);


CREATE TABLE IF NOT EXISTS role (
	role_id INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL,
	role_name VARCHAR(255) NOT NULL,
	icon_id INT REFERENCES media(media_id) NULL,

	created_at TIMESTAMP DEFAULT NOW() NOT NULL,
	audit_isdeleted CHAR(1) DEFAULT 0 NOT NULL,
	version INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS role_permission (
	role_id INT REFERENCES role(role_id) NOT NULL,
	permission_id INT REFERENCES permission(permission_id) NOT NULL,
	created_at TIMESTAMP DEFAULT NOW() NOT NULL,
	audit_isdeleted CHAR(1) DEFAULT 0 NOT NULL,
	version INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS dbuser (
	user_id INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL,
	pfp_id INT REFERENCES media(media_id) NOT NULL,
	username VARCHAR(20) NOT NULL UNIQUE,
	email VARCHAR(255) NOT NULL UNIQUE,
	biography TEXT NULL,
	password_hash VARCHAR(255) NOT NULL,
	password_salt VARCHAR(255) NOT NULL,
	isflagged CHAR(1) DEFAULT 0 NOT NULL,
	isbanned CHAR(1) DEFAULT 0 NOT NULL,
	created_at TIMESTAMP DEFAULT NOW() NOT NULL,
	audit_isdeleted CHAR(1) DEFAULT 0 NOT NULL,
	version INTEGER NOT NULL
);


CREATE TABLE IF NOT EXISTS role_user (
	user_id INT REFERENCES dbuser(user_id) NOT NULL,
	role_id INT REFERENCES role(role_id) NOT NULL,
	created_at TIMESTAMP DEFAULT NOW() NOT NULL,
	audit_isdeleted CHAR(1) DEFAULT 0 NOT NULL,

	version INTEGER NOT NULL
);


CREATE TABLE IF NOT EXISTS supportedlang (
	lang_id INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL,
	lang_name VARCHAR(255) NOT NULL,
	lang_extension VARCHAR(10) NOT NULL,
	icon_id INT REFERENCES media(media_id) NOT NULL,

	created_at TIMESTAMP DEFAULT NOW() NOT NULL,
	audit_isdeleted CHAR(1) DEFAULT 0 NOT NULL,
	version INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS badge (
	badge_id INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL,
	badge_name VARCHAR(255) NOT NULL,
	icon_id INT REFERENCES media(media_id) NOT NULL,
	created_at TIMESTAMP DEFAULT NOW() NOT NULL,
	audit_isdeleted CHAR(1) DEFAULT 0 NOT NULL,
	version INTEGER NOT NULL
);


CREATE TABLE IF NOT EXISTS algorithm (

	algorithm_id INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL,
	owner_id INT REFERENCES dbuser(user_id) NOT NULL,
	title VARCHAR(255) NOT NULL,
	description TEXT NOT NULL,
	created_at TIMESTAMP DEFAULT NOW() NOT NULL,
	audit_isdeleted CHAR(1) DEFAULT 0 NOT NULL,
	version INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS algorithm_collaborator (
	user_id INT REFERENCES dbuser(user_id) NOT NULL,
	algorithm_id INT REFERENCES algorithm(algorithm_id) NOT NULL,
	created_at TIMESTAMP DEFAULT NOW() NOT NULL,
	audit_isdeleted CHAR(1) DEFAULT 0 NOT NULL,

	version INTEGER NOT NULL
);


CREATE TABLE IF NOT EXISTS dbgroup (
	group_id INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL,
	title VARCHAR(255) NOT NULL,
	description TEXT NOT NULL,
	isprivate CHAR(1) NOT NULL,
	owner_id INT REFERENCES dbuser(user_id) NOT NULL,
	created_at TIMESTAMP DEFAULT NOW() NOT NULL,
	audit_isdeleted CHAR(1) DEFAULT 0 NOT NULL,
	version INTEGER NOT NULL

);

CREATE TABLE IF NOT EXISTS group_algorithm (
	group_id INT REFERENCES dbgroup(group_id) NOT NULL,
	algorithm_id INT REFERENCES algorithm(algorithm_id) NOT NULL,
	created_at TIMESTAMP DEFAULT NOW() NOT NULL,
	audit_isdeleted CHAR(1) DEFAULT 0 NOT NULL,
	version INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS algorithm_lang (

	algorithm_id INT REFERENCES algorithm(algorithm_id) NOT NULL,
	lang_id INT REFERENCES supportedlang(lang_id) NOT NULL,
	rootlang_path VARCHAR(255) NOT NULL,
	created_at TIMESTAMP DEFAULT NOW() NOT NULL,
	audit_isdeleted CHAR(1) DEFAULT 0 NOT NULL,
	version INTEGER NOT NULL

);



CREATE TABLE IF NOT EXISTS algorithm_meta (
	algorithm_id INT REFERENCES algorithm(algorithm_id) NOT NULL,
	root_path VARCHAR(255) NOT NULL,
	isprivate CHAR(1) NOT NULL,
	isflagged CHAR(1) DEFAULT 0 NOT NULL,
	isdisabled CHAR(1) DEFAULT 0 NOT NULL,
	created_at TIMESTAMP DEFAULT NOW() NOT NULL,
	audit_isdeleted CHAR(1) DEFAULT 0 NOT NULL,
	version INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS badge_algorithm (
	algorithm_id INT REFERENCES algorithm(algorithm_id) NOT NULL,
	badge_id INT REFERENCES badge(badge_id) NOT NULL,
	created_at TIMESTAMP DEFAULT NOW() NOT NULL,
	audit_isdeleted CHAR(1) DEFAULT 0 NOT NULL,
	version INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS report (
	report_id INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL,
	user_id INT REFERENCES dbuser(user_id) NOT NULL,
	type_id INT REFERENCES content_type(type_id) NOT NULL,
	status_id INT REFERENCES reportstatus(status_id) NOT NULL,
	content_id INTEGER NOT NULL,
	issolved CHAR(1) DEFAULT 0 NOT NULL,
	solver_id INTEGER NULL,
	created_at TIMESTAMP DEFAULT NOW() NOT NULL,
	audit_isdeleted CHAR(1) DEFAULT 0 NOT NULL,
	version INTEGER NOT NULL
);


CREATE TABLE IF NOT EXISTS rating (
	user_id INT REFERENCES dbuser(user_id) NOT NULL,
	type_id INT REFERENCES content_type(type_id) NOT NULL,
	content_id INT NOT NULL,
	created_at TIMESTAMP DEFAULT NOW() NOT NULL,

	audit_isdeleted CHAR(1) DEFAULT 0 NOT NULL,
	version INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS comment (
	comment_id INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL,
	type_id INT REFERENCES content_type(type_id) NOT NULL,
	content_id INTEGER NOT NULL,
	body TEXT NOT NULL,
	replyto_id INT REFERENCES comment(comment_id) NULL,

	owner_id INT REFERENCES dbuser(user_id) NOT NULL,
	created_at TIMESTAMP DEFAULT NOW() NOT NULL,
	audit_isdeleted CHAR(1) DEFAULT 0 NOT NULL,
	version INTEGER NOT NULL

);

CREATE TABLE IF NOT EXISTS tag_algorithm (
	tag_id INT REFERENCES tag(tag_id) NOT NULL,
	algorithm_id INT REFERENCES algorithm(algorithm_id) NOT NULL,
	created_at TIMESTAMP DEFAULT NOW() NOT NULL,
	audit_isdeleted CHAR(1) DEFAULT 0 NOT NULL,
	version INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS moderationlog (
	log_id INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL,
	user_id INT REFERENCES dbuser(user_id) NOT NULL,
	type_id INT REFERENCES content_type(type_id) NOT NULL,
	content_id INTEGER NOT NULL,
	created_at TIMESTAMP DEFAULT NOW() NOT NULL,

	audit_isdeleted CHAR(1) DEFAULT 0 NOT NULL,
	version INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS algorithm_changelog (
	changelog_id INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL,
	algorithm_id INT REFERENCES algorithm(algorithm_id) NOT NULL,

	lang_id INT REFERENCES supportedlang(lang_id) NOT NULL,
	file_path VARCHAR(255) NOT NULL,
	created_at TIMESTAMP DEFAULT NOW() NOT NULL,
	audit_isdeleted CHAR(1) DEFAULT 0 NOT NULL,
	version INTEGER NOT NULL
);

