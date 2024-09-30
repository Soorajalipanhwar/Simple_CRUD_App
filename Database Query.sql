Create SchoolDatabase

use SchoolDatabase

CREATE TABLE Stock (
    item_id INT PRIMARY KEY,
    item_name VARCHAR(100),
    item_quantity INT
);

CREATE TABLE Sales (
    sales_id INT PRIMARY KEY identity(1,1),
    item_id INT,
    item_name VARCHAR(100),
    no_of_items INT,
    unit_price DECIMAL(10, 2),
    FOREIGN KEY (item_id) REFERENCES Stock(item_id)
);


INSERT INTO Stock (item_id, item_name, item_quantity) VALUES 
(1, 'Item1', 100),
(2, 'Item2', 150),
(3, 'Item3', 200);


INSERT INTO Sales (item_id, item_name, no_of_items, unit_price) VALUES
(1, 'Item1', 5, 10.99),
(2, 'Item2', 3, 15.50),
(1, 'Item1', 2, 10.99);

select * from Sales


select * from Stock