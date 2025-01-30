[Back to README](../README.md)

### Sales API

#### HTTP Status Codes
- `200 OK`: Successful operation.
- `201 Created`: Sale created successfully.
- `400 Bad Request`: Validation error or invalid request.
- `404 Not Found`: Resource not found.
- `500 Internal Server Error`: Unexpected server error.

#### POST /api/Sales
- Description: Creates a new sale.
- Request Body:
  ```json
  {
    "saleDate": "date-time",
    "customerId": "integer",
    "customerName": "string",
    "branchId": "integer",
    "branchName": "string",
    "items": [
      {
        "productId": "integer",
        "productName": "string",
        "quantity": "integer",
        "unitPrice": "number"
      }
    ]
  }

- Response (201 Created):
  ```json
  {
  "success": true,
  "message": "Sale created successfully",
  "data": {
    "id": "uuid",
    "saleNumber": "string",
    "saleDate": "date-time",
    "customerId": "integer",
    "customerName": "string",
    "totalAmount": "number",
    "branchId": "integer",
    "branchName": "string",
    "items": [
      {
        "id": "uuid",
        "productId": "integer",
        "productName": "string",
        "quantity": "integer",
        "unitPrice": "number",
        "discount": "number",
        "totalAmount": "number"
      }
    ],
    "isCancelled": "boolean",
    "itemCount": "integer"
    }
  }

- Response (400 Bad Request):
    ```json
    {
    "success": false,
    "message": "",
    "errors": [
        {
        "error": "string",
        "detail": "string"
        }
    ]
    }

#### GET /api/Sales/{id}
- Description: Retrieves a specific sale by ID.
- Path Parameters:
    - id: UUID of the sale.
- Response (200 OK):
    ```json
    {
    "success": true,
    "message": "",
    "data": {
        "id": "uuid",
        "saleNumber": "string",
        "saleDate": "date-time",
        "customerId": "integer",
        "customerName": "string",
        "totalAmount": "number",
        "branchId": "integer",
        "branchName": "string",
        "items": [
        {
            "id": "uuid",
            "productId": "integer",
            "productName": "string",
            "quantity": "integer",
            "unitPrice": "number",
            "discount": "number",
            "totalAmount": "number"
        }
        ],
        "isCancelled": "boolean"
    }
    }

- Response (400 Bad Request): (See example above)
- Response (404 Not Found):
    ```json
    {
    "success": false,
    "message": "Sale not found",
    "errors": []
    }

#### GET /api/Sales
- Description: Retrieves sales with pagination, ordering, and filtering.
- Query Parameters:
    - page  (optional, integer): Número da página (default: 1).
    - size (optional): Number of items per page (default: 10).
    - order  (optional, string): Ordenação dos resultados (asc ou desc, default: asc).
    - filters (optional): Dictionary of filters, examples:
      - filters[customerName] (optional, string): Filters sales by the customer's name or part of the name.
      - filters[branchName] (optional, string): Filters sales by the store or branch name.
      - filters[saleDateStart] (optional, string, format: YYYY-MM-DD): Retrieves sales from this date onward (start of the range).
      - filters[saleDateEnd] (optional, string, format: YYYY-MM-DD): Retrieves sales up to this date (end of the range).
      - filters[saleDate] (optional, string, format: YYYY-MM-DD): Filters sales on a specific date.
      - filters[isCancelled] (optional, boolean): Determines whether to include only canceled (true) or active (false) sales.
- Response (200 OK):
    ```json
    {
    "success": true,
    "message": "",
    "data": {
        "items": [
        {
            "id": "uuid",
            "saleNumber": "string",
            "saleDate": "date-time",
            "customerId": "integer",
            "customerName": "string",
            "totalAmount": "number",
            "branchId": "integer",
            "branchName": "string",
            "isCancelled": "boolean"
        }
        ],
        "currentPage": "integer",
        "totalPages": "integer",
        "pageSize": "integer",
        "totalCount": "integer",
        "hasPrevious": "boolean",
        "hasNext": "boolean"
    }
    }

- Response (400 Bad Request): (See example above)

PUT /api/Sales/{id}
- Description: Updates an existing sale.
- Path Parameters:
    - id: UUID of the sale to update.
- Request Body: (Same as POST /api/Sales)
- Response (200 OK): (Same as GET /api/Sales/{id})
- Response (400 Bad Request): (See example above)
- Response (404 Not Found): (See example above, but message might be different)

#### DELETE /api/Sales/{id}
- Description: Deletes a sale by its ID.
- Path Parameters:
  - id: UUID of the sale to delete.
- Response (200 OK):
    ```json
    {
    "success": true,
    "message": "Sale cancelled successfully",
    "errors": []
    }

- Response (400 Bad Request): (See example above)
- Response (404 Not Found): (See example above, but message might be different)

---
**[? Previous: Products API](./products-api.md) | [Next: General API ?](./general-api.md)**
