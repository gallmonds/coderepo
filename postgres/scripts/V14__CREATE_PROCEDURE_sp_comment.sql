CREATE OR REPLACE PROCEDURE public.sp_comment(
    IN p_content_id integer,
    IN p_dbuser_id integer,
    IN p_body TEXT,
    IN p_replyto_id integer default null
)
LANGUAGE plpgsql
AS $procedure$
DECLARE
	v_isreplyvalid BOOLEAN;
	v_iscommentvalid BOOLEAN;
BEGIN
    IF NOT EXISTS (SELECT 1 FROM role_user ru WHERE ru.user_id = p_dbuser_id) THEN
        RAISE EXCEPTION 'The user % does not have the required permissions to perform this action, or the user does not exist.', p_dbuser_id
        USING ERRCODE = 'PER03';
    END IF;
	
	IF NOT EXISTS (
		SELECT 1
		FROM algorithm al
		JOIN algorithm_meta am
		ON am.algorithm_id = al.algorithm_id 
		WHERE al.algorithm_id = p_content_id
		AND al.audit_isdeleted = '0'::bpchar) THEN
		
		RAISE EXCEPTION 'The codelet does not exist.'
        USING ERRCODE = 'PER03';
	END IF;
	
	IF p_replyto_id IS NOT NULL
	THEN
		IF NOT EXISTS (SELECT 1 FROM comment WHERE comment_id = p_replyto_id AND audit_isdeleted != '1'::bpchar) THEN
			RAISE EXCEPTION 'The specified user does not exist.'
        	USING ERRCODE = 'PER03';
		END IF;
	END IF;
	

	INSERT INTO comment (type_id, content_id, body, replyto_id, owner_id, version)
	VALUES (2, p_content_id, p_body, p_replyto_id, p_dbuser_id, 1);

	

EXCEPTION
    WHEN OTHERS THEN
        RAISE;
END;
$procedure$;