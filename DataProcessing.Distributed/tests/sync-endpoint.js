import http from 'k6/http';
import { check } from 'k6';

export const options = {
  vus: 50,
  duration: '30s',
};

const BASE_URL = __ENV.BASE_URL || 'http://localhost:5002';

export default function () {
  const payload = JSON.stringify({
    source: `sensor-${Math.floor(Math.random() * 1000)}`,
    value: Math.random() * 100,
  });

  const params = {
    headers: {
      'Content-Type': 'application/json',
    },
  };

  const response = http.post(`${BASE_URL}/api/measurements/sync`, payload, params);

  check(response, {
    'status is 200': (r) => r.status === 200,
  });
}
