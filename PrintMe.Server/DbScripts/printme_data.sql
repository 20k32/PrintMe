INSERT INTO user_status (status)
VALUES ('ACTIVE'),
       ('INACTIVE'),
       ('BLOCKED');

INSERT INTO request_type (type)
VALUES ('USER_REPORT'),
       ('PRINTER_APPLICATION'),
       ('ACCOUNT_DELETION'),
       ('PRINTER_DESCRIPTION_CHANGE');

INSERT INTO request_status (status)
VALUES ('PENDING'),
       ('ACCEPTED'),
       ('DECLINED');

INSERT INTO request_status_reason (reason)
VALUES ('NO_REASON'),
       ('USER_DELETED'),
       ('USER_BANNED');

INSERT INTO print_order_status (status)
VALUES ('PENDING'),
       ('ACCEPTED'),
       ('DECLINED'),
       ('IN_PROGRESS'),
       ('COMPLETED'),
       ('CANCELLED'),
       ('ARCHIVED');

INSERT INTO print_order_status_reason (reason)
VALUES ('INAPPROPRIATE_CONTENT'),
       ('ABUSIVE_BEHAVIOUR'),
       ('ABSENT_MATERIALS');

INSERT INTO print_material (name)
VALUES ('PLA'),
       ('ABS'),
       ('PETG'),
       ('NYLON');
