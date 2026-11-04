# Cinevibe frontend

Angular 20 standalone frontend for the Movie Ticket Booking API, styled with Bootstrap 5.

## Run locally

1. Use Node.js 20.19+, 22.12+, or 24+.
2. Start the C# API using the `http` launch profile (`http://localhost:5173`).
3. From this folder, install dependencies and start Angular:

   ```sh
   npm install
   npm start
   ```

4. Open `http://localhost:4200`.

The Angular development server proxies `/api` requests to the local C# API. If the API uses a different URL, update `proxy.conf.json`.

## Features

- Browse, search, and page through active movies.
- View upcoming showtimes and screen seats, then submit a booking.
- Register or log in and look up a booking by its reference number.

The API currently returns screen seat inventory rather than per-show seat status. Seat availability is validated by the booking API when a booking is submitted.

## Production build

```sh
npm run build
```
