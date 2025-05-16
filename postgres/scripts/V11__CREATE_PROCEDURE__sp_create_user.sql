CREATE OR REPLACE PROCEDURE create_user(
    username TEXT,
    email TEXT,
    password_hash TEXT,
    password_salt TEXT
)
LANGUAGE plpgsql
AS $$
DECLARE
    user_count INT;
BEGIN
    SELECT COUNT(*) INTO user_count
    FROM dbuser
    WHERE username = username;
    
    IF user_count > 0 THEN
        RAISE EXCEPTION 'USR01: The username already exists.' USING ERRCODE = 'USR01';
    END IF;

    SELECT COUNT(*) INTO user_count
    FROM dbuser
    WHERE email = email;
    
    IF user_count > 0 THEN
        RAISE EXCEPTION 'USR01: The email already exists.' USING ERRCODE = 'USR01';
    END IF;

    INSERT INTO dbuser (
        pfp_id, 
        username, 
        email, 
        password_hash, 
        password_salt,
		isflagged,
		isbanned, 
        audit_isdeleted, 
        version
    ) 
    VALUES (
        NULL, 
        username, 
        email, 
        password_hash, 
        password_salt,
		0,
		0, 
        0,
        1
    );
    
END;
$$;
