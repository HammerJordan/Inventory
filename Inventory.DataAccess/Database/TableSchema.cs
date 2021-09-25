namespace Inventory.DataAccess
{
    public static class TableSchema
    {
        public const string CREATE_PRODUCT_SCHEMA =
            @"create table Product
        (
            ID INTEGER not null primary key autoincrement unique,
            Name TEXT,
            Description TEXT,
            UPC TEXT,
            Cost REAL not null,
            Unit TEXT,
            URL TEXT,
            LastUpdated TEXT,
            ImageHref TEXT
        );";

        public const string CREATE_CATEGORY_SCHEMA =
            @"create table Category
        (
            ID INTEGER not null
                constraint Category_pk
                    primary key autoincrement unique ,
            ParentID int,
            Name TEXT not null,
            FOREIGN KEY (ParentID) REFERENCES Category(ID)
        );";

        public const string CREATE_PRODUCT_CATEGORY_SCHEMA =
            @"create table Product_Category
        (
            ProductID INTEGER not null,
            ProductCategoryID INTEGER not null,
            foreign key (ProductID) references Product(ID),
            foreign key (ProductCategoryID) references Category(ID)
        );";

        public const string CREATE_RECORD_SCHEMA =
            @"create table Record
        (
	        ID INTEGER not null
		        primary key autoincrement
		        unique,
	        Name TEXT not null,
	        CreatedDateTime TEXT
        );";

        public const string CREATE_RECORD_ITEM_SCHEMA =
            @"create table RecordItem
        (
	        ID INTEGER not null
		        primary key autoincrement
		        unique,
	        RecordID INTEGER not null
		        references Record,
	        ProductID INTEGER not null
		        references Product,
	        Quantity INTEGER not null
        );";
    }
}