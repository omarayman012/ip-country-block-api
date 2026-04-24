 Assignment Requirements Implementation

This section summarizes how all requirements of the test assignment were implemented.

---

 1. Find My Country via IP Lookup

 Implemented endpoint:
`GET /api/ip/lookup?ipAddress={ip}`

 Features implemented:
- Calls third-party API (ipapi.co) to retrieve:
  - Country Code
  - Country Name
  - ISP
- Validates IP format using `IPAddress.TryParse`
- If `ipAddress` is not provided:
  - Automatically detects client IP using `HttpContext`
  - Supports `X-Forwarded-For` header for real-world scenarios
- Graceful fallback handling for API failures

---

 2. Verify If IP is Blocked

 Implemented endpoint:
`GET /api/ip/check-block`

 Steps implemented:
1. Automatically fetches caller IP using `HttpContext`
2. Calls external geolocation API to resolve country information
3. Checks if the country exists in:
   - Permanently blocked countries list
   - Temporarily blocked countries list
4. Logs every check attempt (blocked or not)
5. Returns structured response indicating block status

---

 3. Log Blocked Attempts

 Implemented endpoint:
`GET /api/logs/blocked-attempts`

 Features:
- Stores logs in-memory using thread-safe collections
- Each log entry contains:
  - IP Address
  - Country Code
  - Timestamp
  - Blocked Status
  - User Agent
- Supports pagination (`page`, `pageSize`)
- Ensures performance using LINQ pagination (`Skip/Take`)

---

 4. Temporarily Block a Country

 Implemented endpoint:
`POST /api/countries/temporal-block`

 Features:
- Blocks a country for a specific duration (in minutes)
- Validates:
  - Duration must be between 1 and 1440 minutes
  - Rejects invalid country codes (e.g., "XX")
  - Prevents duplicate temporary blocks (returns 409 Conflict)
- Stores temporary blocks in-memory with expiration time
- Uses background service to automatically remove expired blocks every 5 minutes

---

 Background Service

 Implemented:
`TemporalBlockCleanupService`

 Responsibilities:
- Runs every 5 minutes
- Removes expired temporary blocks automatically
- Ensures data consistency without manual intervention

---
 Technical Highlights

-  Clean Architecture (Domain / Application / Infrastructure / API)
-  In-Memory Storage using ConcurrentDictionary
-  HttpClientFactory for external API calls
-  Async/Await throughout the system
-  Separation of concerns (Services, Interfaces, Controllers)
-  Centralized logging system
-  Swagger documentation enabled


 Notes

- The system is fully stateless (no database used)
- Designed to simulate real-world scalable API architecture
- Focus on clean separation between business logic and infrastructure
- Built with extensibility in mind (easy to replace in-memory with database later)
