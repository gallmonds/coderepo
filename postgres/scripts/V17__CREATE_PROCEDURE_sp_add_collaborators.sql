CREATE OR REPLACE PROCEDURE public.sp_add_collaborators(
    IN p_users integer[],
    IN p_dbuser_id integer,
    IN p_algorithm_id integer
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_user_id integer;
BEGIN
    IF NOT EXISTS (SELECT 1 FROM role_user ru WHERE ru.user_id = p_dbuser_id) THEN
        RAISE EXCEPTION 'The user % does not have the required permissions to perform this action, or the user does not exist.', p_dbuser_id
        USING ERRCODE = 'PER03';
    END IF;

    IF NOT EXISTS (SELECT 1 FROM algorithm al 
                   WHERE al.algorithm_id = p_algorithm_id 
                   AND al.audit_isdeleted = '0'::bpchar
                   AND al.owner_id = p_dbuser_id) THEN
        RAISE EXCEPTION 'The specified algorithm does not exist or the user is not owner of it.'
        USING ERRCODE = 'PER05';
    END IF;

    FOREACH v_user_id IN ARRAY p_users
    LOOP
        IF NOT EXISTS (SELECT 1 FROM role_user ru WHERE ru.user_id = v_user_id) THEN
            RAISE EXCEPTION 'One of the users does not have the required permissions to perform this action, or the user does not exist.'
            USING ERRCODE = 'PER09';
        END IF;

        INSERT INTO algorithm_collaborator (user_id, algorithm_id, version)
        VALUES (v_user_id, p_algorithm_id, 1);
    END LOOP;

EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;
$$;