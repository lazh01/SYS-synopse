# K6 Load Tests

This folder contains stress tests for the three communication pattern endpoints in the ingestion service.

## Tests

1. **sync-endpoint.js** - Tests the synchronous HTTP endpoint (`/api/measurements/sync`)
2. **async-endpoint.js** - Tests the asynchronous HTTP endpoint (`/api/measurements/async`)
3. **broker-endpoint.js** - Tests the message broker endpoint (`/api/measurements/broker`)

## Running the Tests

### Prerequisites

Install k6:
```bash
# macOS
brew install k6

# Windows (using Chocolatey)
choco install k6

# Linux
sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys C5AD17C747E3415A3642D57D77C6C491D6AC1D69
echo "deb https://dl.k6.io/deb stable main" | sudo tee /etc/apt/sources.list.d/k6.list
sudo apt-get update
sudo apt-get install k6
```

### Running Individual Tests

```bash
# Test synchronous endpoint
k6 run tests/sync-endpoint.js

# Test asynchronous endpoint
k6 run tests/async-endpoint.js

# Test message broker endpoint
k6 run tests/broker-endpoint.js
```

### Running with Custom Base URL

```bash
BASE_URL=http://localhost:5002 k6 run tests/sync-endpoint.js
```

### Running All Tests Sequentially

```bash
k6 run tests/sync-endpoint.js && \
k6 run tests/async-endpoint.js && \
k6 run tests/broker-endpoint.js
```

## Test Configuration

Each test uses a simple stress test pattern:
- 50 virtual users
- 30 seconds duration

**Total test duration: 30 seconds per test**

## Metrics

Each test uses k6's built-in metrics:
- `http_req_duration` - Request duration
- `http_req_failed` - Failed request rate
- `http_reqs` - Total number of requests
- `vus` - Virtual users

