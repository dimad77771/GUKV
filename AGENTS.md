# Project invariants

- Legacy object and rent-agreement IDs can be negative. A negative ID is valid and must not be rejected with `id <= 0` or `id > 0` checks. Validate existence and relationships from the database instead of relying on the sign of these IDs.
