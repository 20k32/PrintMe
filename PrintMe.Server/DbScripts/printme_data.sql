INSERT INTO user_status (status)
VALUES ('Active'),
       ('Inactive'),
       ('Blocked');
INSERT INTO request_type (type)
VALUES ('User report'),
       ('Printer application'),
       ('Account deletion'),
       ('Printer description change');
INSERT INTO request_status (status)
VALUES ('Pending'),
       ('Accepted'),
       ('Declined');
INSERT INTO request_status_reason (reason)
VALUES ('No reason'),
       ('User deleted'),
       ('User banned');
INSERT INTO print_order_status (status)
VALUES ('Pending'),
       ('Accepted'),
       ('Declined'),
       ('In progress'),
       ('Completed'),
       ('Cancelled'),
       ('Archived');
INSERT INTO print_order_status_reason (reason)
VALUES ('Inappropriate content'),
       ('Abusive behaviour'),
       ('Absent materials');
INSERT INTO print_material (name)
VALUES ('PLA'),
       ('ABS'),
       ('PETG'),
       ('Nylon');