CREATE OR REPLACE PROCEDURE sp_update_profile_picture(
    IN p_user_id     INTEGER,
    IN p_file_path   TEXT,
    IN p_mime_type   TEXT
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_media_id       INTEGER;
    v_old_pfp_id     INTEGER;
BEGIN

    IF NOT EXISTS (
        SELECT 1 FROM role_user ru
        WHERE ru.user_id = p_user_id
    ) THEN
        RAISE EXCEPTION
            'The user % does not have the required permissions to perform this action, or the user does not exist.',
            p_user_id
            USING ERRCODE = 'PER03';
    END IF;


    SELECT pfp_id
    INTO   v_old_pfp_id
    FROM   dbuser
    WHERE  user_id = p_user_id;


    INSERT INTO media
           (file_path, mime_type, category_id, created_at, audit_isdeleted, version)
    VALUES (p_file_path, p_mime_type, 1        , now()    , '0', 1)
    RETURNING media_id INTO v_media_id;

    UPDATE dbuser
    SET    pfp_id = v_media_id
    WHERE  user_id = p_user_id;

    IF v_old_pfp_id IS NOT NULL THEN
        UPDATE media
        SET    audit_isdeleted = '1'
        WHERE  media_id = v_old_pfp_id;
    END IF;
END;
$$;
