CREATE OR REPLACE PROCEDURE public.sp_add_language(
    IN p_algorithm_id INT, 
    IN p_dbuser_id INT, 
    IN p_supportedlang_id INT
)
LANGUAGE plpgsql
AS $procedure$
DECLARE
    v_versionnumber INT;
	v_langpath TEXT;
	v_langextension TEXT;
	v_filepath TEXT;
BEGIN
    BEGIN
        IF EXISTS (SELECT 1 FROM role_user ru WHERE ru.user_id = p_dbuser_id) THEN

            IF EXISTS (
                SELECT 1
                FROM algorithm al
                LEFT JOIN algorithm_collaborator ac ON al.algorithm_id = ac.algorithm_id
                WHERE ((al.algorithm_id = p_algorithm_id AND al.owner_id = p_dbuser_id)
			    OR (ac.algorithm_id = p_algorithm_id AND ac.user_id = p_dbuser_id))
			    AND al.audit_isdeleted = '0'::bpchar
            ) THEN
                SELECT COUNT(*) + 1
                INTO v_versionnumber
                FROM algorithm_lang algl
                WHERE algl.algorithm_id = p_algorithm_id;
				
				SELECT lang_extension
				INTO v_langextension
				FROM supportedlang sl
				WHERE sl.lang_id = p_supportedlang_id;

				v_langpath := p_algorithm_id::TEXT || '/' || v_langextension;
				v_filepath := v_langpath || '/' || v_versionnumber::TEXT || '.' || v_langextension;

				IF NOT EXISTS (
					SELECT 1
					FROM algorithm_lang aglg
					WHERE algorithm_id = p_algorithm_id AND lang_id = p_supportedlang_id
				) THEN
					INSERT INTO algorithm_lang (algorithm_id, lang_id, rootlang_path, version)
					VALUES (p_algorithm_id, p_supportedlang_id, v_langpath, 1);
				END IF;
			
				INSERT INTO algorithm_changelog (algorithm_id, lang_id, file_path, version)
				VALUES (p_algorithm_id, p_supportedlang_id, v_filepath, 1);

            ELSE
                RAISE EXCEPTION 'The user % is not owner nor collaborator of the specified codelet, or the codelet does not exist.', p_dbuser_id USING ERRCODE = 'PER02';
            END IF;
            
        ELSE
            RAISE EXCEPTION 'The user % does not have the required permissions to perform this action, or the user does not exist.', p_dbuser_id USING ERRCODE = 'PER01';
        END IF;
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            RAISE;
    END;
END;
$procedure$;