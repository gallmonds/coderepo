-- Codelets
INSERT INTO permission(permission_name, version) VALUES ('CODELET_CREATE', 1);
INSERT INTO permission(permission_name, version) VALUES ('CODELET_UPDATE_OWN', 1);
INSERT INTO permission(permission_name, version) VALUES ('CODELET_DELETE_OWN', 1);
INSERT INTO permission(permission_name, version) VALUES ('CODELET_SEND_CONTRIBUTION', 1);


-- Users
INSERT INTO permission(permission_name, version) VALUES ('USER_CREATE_REPORT', 1);
INSERT INTO permission(permission_name, version) VALUES ('USER_UPDATE_OWN', 1);
INSERT INTO permission(permission_name, version) VALUES ('USER_DELETE_OWN', 1);


-- Groups
INSERT INTO permission(permission_name, version) VALUES ('GROUP_CREATE', 1);
INSERT INTO permission(permission_name, version) VALUES ('GROUP_UPDATE_OWN', 1);
INSERT INTO permission(permission_name, version) VALUES ('GROUP_DELETE_OWN', 1);
INSERT INTO permission(permission_name, version) VALUES ('GROUP_ADD_CODELET', 1);
INSERT INTO permission(permission_name, version) VALUES ('GROUP_REMOVE_CODELET', 1);


-- Comment
INSERT INTO permission(permission_name, version) VALUES ('COMMENT_CODELETS', 1);
INSERT INTO permission(permission_name, version) VALUES ('COMMENT_PROFILE', 1);
INSERT INTO permission(permission_name, version) VALUES ('COMMENT_GROUP', 1);
INSERT INTO permission(permission_name, version) VALUES ('COMMENT_REPLY_CODELETS', 1);
INSERT INTO permission(permission_name, version) VALUES ('COMMENT_REPLY_PROFILE', 1);
INSERT INTO permission(permission_name, version) VALUES ('COMMENT_REPLY_GROUP', 1);
INSERT INTO permission(permission_name, version) VALUES ('COMMENT_UPDATE_OWN', 1);
INSERT INTO permission(permission_name, version) VALUES ('COMMENT_DELETE_OWN_CODELET', 1);
INSERT INTO permission(permission_name, version) VALUES ('COMMENT_DELETE_OWN_PROFILE', 1);
INSERT INTO permission(permission_name, version) VALUES ('COMMENT_DELETE_OWN_GROUP', 1);


-- Rate
INSERT INTO permission(permission_name, version) VALUES ('RATE_CODELET', 1);
INSERT INTO permission(permission_name, version) VALUES ('RATE_GROUP', 1);


-- Tag
INSERT INTO permission(permission_name, version) VALUES ('TAG_CREATE', 1);


-- PRIVILEGED
INSERT INTO permission(permission_name, version) VALUES ('CODELET_UPDATE_OTHERS', 1);
INSERT INTO permission(permission_name, version) VALUES ('CODELET_DELETE_OTHERS', 1);
INSERT INTO permission(permission_name, version) VALUES ('CODELET_FLAG', 1);
INSERT INTO permission(permission_name, version) VALUES ('CODELET_DISABLE', 1);

INSERT INTO permission(permission_name, version) VALUES ('USER_FLAG_USER', 1);
INSERT INTO permission(permission_name, version) VALUES ('USER_BAN_USER', 1);
INSERT INTO permission(permission_name, version) VALUES ('USER_BAN_ASSISTANT', 1);
INSERT INTO permission(permission_name, version) VALUES ('USER_BAN_ADMIN', 1);
INSERT INTO permission(permission_name, version) VALUES ('USER_DELETE_OTHERS', 1);
INSERT INTO permission(permission_name, version) VALUES ('USER_UPDATE_OTHERS', 1);

INSERT INTO permission(permission_name, version) VALUES ('GROUP_DELETE_OTHERS', 1);
INSERT INTO permission(permission_name, version) VALUES ('GROUP_UPDATE_OTHERS', 1);

INSERT INTO permission(permission_name, version) VALUES ('USER_PROMO_ASSISTANT', 1);
INSERT INTO permission(permission_name, version) VALUES ('USER_PROMO_ADMIN', 1);

INSERT INTO permission(permission_name, version) VALUES ('COMMENT_DELETE_OTHERS', 1);
INSERT INTO permission(permission_name, version) VALUES ('COMMENT_UPDATE_OTHERS', 1);

INSERT INTO permission(permission_name, version) VALUES ('BADGE_GIVE_USER', 1);
INSERT INTO permission(permission_name, version) VALUES ('BADGE_GIVE_CODELET', 1);

INSERT INTO permission(permission_name, version) VALUES ('REPORT_TAKE', 1);
INSERT INTO permission(permission_name, version) VALUES ('REPORT_ASIGN', 1);
INSERT INTO permission(permission_name, version) VALUES ('REPORT_SOLVE', 1);
INSERT INTO permission(permission_name, version) VALUES ('REPORT_DELETE', 1);

INSERT INTO permission(permission_name, version) VALUES ('MODLOG_VIEW', 1);

INSERT INTO permission(permission_name, version) VALUES ('TAG_GIVE_CODELET', 1);
INSERT INTO permission(permission_name, version) VALUES ('TAG_UPDATE', 1);
INSERT INTO permission(permission_name, version) VALUES ('TAG_GIVE_CODELET_OTHERS', 1);
INSERT INTO permission(permission_name, version) VALUES ('TAG_REMOVE_CODELET_OTHERS', 1);
