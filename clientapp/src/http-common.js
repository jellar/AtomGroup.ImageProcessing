﻿import axios from 'axios';

export default axios.create({
    baseURL: "https://localhost:5001/",
    headers: {
        "Content-Type": "application/json"
    }
})