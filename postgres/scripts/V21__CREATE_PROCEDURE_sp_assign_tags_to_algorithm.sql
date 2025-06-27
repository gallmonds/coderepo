CREATE OR REPLACE PROCEDURE sp_assign_tags_to_algorithm(
    IN p_algorithm_id INT,
    IN p_dbuser_id INT,
    IN p_tag_ids INT[]
)
LANGUAGE plpgsql
AS $$
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM algorithm a
        WHERE a.algorithm_id = p_algorithm_id AND a.owner_id = p_dbuser_id
    ) AND NOT EXISTS (
        SELECT 1
        FROM algorithm_collaborator ac
        WHERE ac.algorithm_id = p_algorithm_id AND ac.user_id = p_dbuser_id
    ) THEN
        RAISE EXCEPTION 'The user % does not have permission to modify this algorithm or the user does not exist.', p_dbuser_id
        USING ERRCODE = 'PER03';
    END IF;

    INSERT INTO tag_algorithm (algorithm_id, tag_id, version)
	SELECT p_algorithm_id, tag_id, 1
	FROM unnest(p_tag_ids) AS tag_id
	WHERE NOT EXISTS (
	    SELECT 1
	    FROM tag_algorithm ta
	    WHERE ta.algorithm_id = p_algorithm_id AND ta.tag_id = tag_id
	);

END;
$$;