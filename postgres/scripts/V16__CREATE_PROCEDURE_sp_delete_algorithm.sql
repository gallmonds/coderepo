CREATE OR REPLACE PROCEDURE public.sp_delete_algorithm(
    IN p_algorithm_id integer,
	IN p_dbuser_id integer
)
LANGUAGE plpgsql
AS $procedure$
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
	
	UPDATE algorithm
	SET audit_isdeleted = '1'::bpchar
	WHERE algorithm_id = p_algorithm_id;

	UPDATE algorithm_meta
	SET audit_isdeleted = '1'::bpchar
	WHERE algorithm_id = p_algorithm_id;	
	
	UPDATE algorithm_lang
	SET audit_isdeleted = '1'::bpchar
	WHERE algorithm_id = p_algorithm_id;

	UPDATE algorithm_changelog
	SET audit_isdeleted = '1'::bpchar
	WHERE algorithm_id = p_algorithm_id;	

	UPDATE algorithm_collaborator
	SET audit_isdeleted = '1'::bpchar
	WHERE algorithm_id = p_algorithm_id;
	
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;
$procedure$;