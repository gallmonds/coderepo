CREATE OR REPLACE PROCEDURE public.sp_rate_content(
    IN p_content_id integer,
    IN p_dbuser_id integer,
    IN p_type_id integer
)
LANGUAGE plpgsql
AS $procedure$
DECLARE
    v_content_exists BOOLEAN;
    v_rating_exists BOOLEAN;
    v_is_deleted BOOLEAN;
    v_table_name TEXT;
    v_content_check_sql TEXT;
BEGIN
    IF NOT EXISTS (SELECT 1 FROM role_user ru WHERE ru.user_id = p_dbuser_id) THEN
        RAISE EXCEPTION 'The user % does not have the required permissions to perform this action, or the user does not exist.', p_dbuser_id
        USING ERRCODE = 'PER03';
    END IF;

    CASE p_type_id
        WHEN 1 THEN
            v_table_name := 'comment';
            v_content_check_sql := 'SELECT EXISTS(SELECT 1 FROM comment WHERE comment_id = $1 AND audit_isdeleted = ''0'')';
        WHEN 2 THEN
            v_table_name := 'algorithm';
            v_content_check_sql := 'SELECT EXISTS(SELECT 1 FROM algorithm WHERE algorithm_id = $1 AND audit_isdeleted = ''0'')';
        ELSE
            RAISE EXCEPTION 'The specified type of content does not exist or it is not available right now.' 
            USING ERRCODE = 'PER01';
    END CASE;

    EXECUTE v_content_check_sql USING p_content_id INTO v_content_exists;

    IF NOT v_content_exists THEN
        RAISE EXCEPTION 'The specified % does not exist or you do not have permissions.', v_table_name
        USING ERRCODE = 'PER02';
    END IF;

    SELECT EXISTS(
        SELECT 1
        FROM rating ra
        WHERE ra.type_id = p_type_id
        AND ra.content_id = p_content_id
        AND ra.user_id = p_dbuser_id
    ) INTO v_rating_exists;

    IF NOT v_rating_exists THEN
        INSERT INTO rating(user_id, type_id, content_id, version)
        VALUES (p_dbuser_id, p_type_id, p_content_id, 1);
    ELSE
        SELECT ra.audit_isdeleted = '1'::bpchar
        INTO v_is_deleted
        FROM rating ra
        WHERE ra.type_id = p_type_id
        AND ra.content_id = p_content_id
        AND ra.user_id = p_dbuser_id
        LIMIT 1;

        IF v_is_deleted THEN
            UPDATE rating
            SET audit_isdeleted = '0'::bpchar
            WHERE type_id = p_type_id
            AND content_id = p_content_id
            AND user_id = p_dbuser_id;
        ELSE
            UPDATE rating
            SET audit_isdeleted = '1'::bpchar
            WHERE type_id = p_type_id
            AND content_id = p_content_id
            AND user_id = p_dbuser_id;
        END IF;
    END IF;

EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;
$procedure$;