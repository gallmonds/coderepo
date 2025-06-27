CREATE OR REPLACE PROCEDURE public.sp_update_user_profile(
    IN p_dbuser_id INTEGER,
    IN p_username TEXT,
    IN p_biography TEXT
)
LANGUAGE plpgsql
AS $procedure$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM role_user ru WHERE ru.user_id = p_dbuser_id) THEN
        RAISE EXCEPTION 'The user % does not have the required permissions to perform this action, or the user does not exist.', p_dbuser_id
        USING ERRCODE = 'PER03';
    END IF;

    IF EXISTS (
        SELECT 1 FROM dbuser du
        WHERE du.username = p_username AND du.user_id <> p_dbuser_id
    ) THEN
        RAISE EXCEPTION 'Username already in use.'
        USING ERRCODE = 'PER10';
    END IF;

    UPDATE dbuser 
    SET username = p_username,
        biography = p_biography
    WHERE user_id = p_dbuser_id;

EXCEPTION
    WHEN OTHERS THEN
        RAISE;
END;
$procedure$;
