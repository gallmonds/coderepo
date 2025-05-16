CREATE OR REPLACE PROCEDURE sp_create_algorithm(
    algorithm_title VARCHAR(255), 
    algorithm_description TEXT, 
    algorithm_isprivate CHAR(1), 
    dbuser_id INT
)
LANGUAGE plpgsql
AS $$
DECLARE
    new_algorithm_id INT;
	algorithm_route TEXT;
BEGIN
    IF EXISTS (SELECT 1 FROM role_user ru WHERE ru.user_id = dbuser_id) THEN
        INSERT INTO algorithm(title, description, owner_id, version)
        VALUES (algorithm_title, algorithm_description, dbuser_id, 1)

        RETURNING algorithm_id INTO new_algorithm_id;
		
		algorithm_route := format('codelet/%s', new_algorithm_id);

        INSERT INTO algorithm_meta(algorithm_id, root_path, isprivate, version)
        VALUES (new_algorithm_id, algorithm_route, algorithm_isprivate, 1);
    ELSE
        RAISE EXCEPTION 'The user % does not have the required permissions to perform this action.', dbuser_id USING ERRCODE = 'PER01';
    END IF;
END;
$$;
