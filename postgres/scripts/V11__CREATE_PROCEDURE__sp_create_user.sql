-- DROP PROCEDURE public.create_user(text, text, text, text);

CREATE OR REPLACE PROCEDURE public.create_user(IN p_username text, IN p_email text, IN p_password_hash text, IN p_password_salt text)
 LANGUAGE plpgsql
AS $procedure$
DECLARE
    user_count INT;
	created_user INT;
BEGIN
    SELECT COUNT(*) INTO user_count
    FROM dbuser
    WHERE username = p_username;
    
    IF user_count > 0 THEN

        RAISE EXCEPTION 'USR01: The username already exists.' USING ERRCODE = 'USR01';
    END IF;

    SELECT COUNT(*) INTO user_count
    FROM dbuser

    WHERE email = p_email;
    
    IF user_count > 0 THEN
        RAISE EXCEPTION 'USR01: The email already exists.' USING ERRCODE = 'USR01';
    END IF;

    -- Insert new user
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
        1, 
        p_username, 
        p_email, 
        p_password_hash, 
        p_password_salt,
        0,
        0, 
        0,
        1
    ) RETURNING user_id INTO created_user;
	
	INSERT INTO role_user(user_id, role_id, version) VALUES (created_user, 1, 1);
END;
$procedure$
;
