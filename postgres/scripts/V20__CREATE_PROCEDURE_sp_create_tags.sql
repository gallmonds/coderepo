CREATE OR REPLACE PROCEDURE sp_create_tags (
    p_tag_names   VARCHAR[],
    p_dbuser_id   INT
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_name   VARCHAR(255);
BEGIN

    IF NOT EXISTS (
        SELECT 1 FROM role_user ru WHERE ru.user_id = p_dbuser_id
    ) THEN
        RAISE EXCEPTION
            'The user % does not have the required permissions to perform this action.',
            p_dbuser_id
            USING ERRCODE = 'PER01';
    END IF;

    FOREACH v_name IN ARRAY p_tag_names LOOP
        v_name := trim(v_name);

        IF v_name IS NULL OR v_name = '' THEN
            CONTINUE;
        END IF;

        IF NOT EXISTS (
            SELECT 1 FROM tag t WHERE lower(t.tag_name) = lower(v_name)
        ) THEN
            INSERT INTO tag (tag_name, version) VALUES (v_name, 1);
        END IF;
    END LOOP;
END
$$;
