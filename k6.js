import http from 'k6/http';
import {sleep} from 'k6';

export const options = {
    stages: [
        { duration: '30s', target: 10 },
        { duration: '1m', target: 50 },
        { duration: '30s', target: 100 },
        { duration: '2m', target: 100 },
        { duration: '30s', target: 0 }
    ],
    thresholds: {
        http_req_duration: ['p(95)<=3000'],
        http_req_failed: ['rate<0.01']
    }
};

export default function(){
    const serverURI = 'http://localhost:5170'
    const stationId = [
        1,
        2,
        3,
        4,
        5,
        6,
        7,
        8,
        9,
        10,
        11,
        12
    ]
    let params = {
        headers: {
            'Content-Type': 'application/json',
            'Origin': 'http://localhost:4200',
            'Authorization': "Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9zaWQiOiIxIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6Im1xdGhhbmdnZyIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6InVzZXIiLCJleHAiOjE3NTU2NjUyNjYsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTE4MCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTE3MCJ9.yGEF9khS9syVmFWE3_H2TxbMe0iqEfwVqnjogjx2jiFqwXcki305KnmPRs08JTiZbAeMutBnTRY0O0g5K2wseM-44yoF4_vtLScKSVWqwrBLCCK5xAhh0j9k3UBE6YMIfqI7NitRbjwsOV3ud9nHC62xn8YelTQAFuT4X33gHlb4b36w_D81rZYcQVlw9hbOcrbQ4N8k7KTrw_IdYDyMM-PU0jOsk2K5qEhDRZjLwjAVjtB5aaReoMnpaXmNH2fugnyw5MqXdavyBTn-LA5bylYNeYqsUGaE0-4PuOhpT4tMjr44uug66k3UcIMiHqnWuxH5iUwAkawbP_fH4HdHYg"
        }
    }
    const filter = {
        "stationId": stationId[Math.floor(Math.random() * stationId.length)],
        "name": null,
        "fuelName": null,
        "logType": null,
        "fromDate": null,
        "toDate": null,
        "fromPrice": null,
        "toPrice": null,
        "fromAmount": null,
        "toAmount": null,
        "fromLiter": null,
        "toLiter": null
    };
    http.post(
        serverURI + `/get/full/filter?page=${Math.floor(Math.random() * 79848)}&pageSize=20`,
        JSON.stringify(filter),
        params
    )
    sleep(1)
}