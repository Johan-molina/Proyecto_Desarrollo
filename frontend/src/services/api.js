import axios from 'axios';

// Verifica que este puerto coincida con el de tu backend
const API_URL = 'http://localhost:5113/api';

console.log('🔌 API configurada para:', API_URL);

const api = axios.create({
    baseURL: API_URL,
    headers: {
        'Content-Type': 'application/json',
    },
    timeout: 5000, // 5 segundos de timeout
});

// Interceptor para ver todas las peticiones
api.interceptors.request.use(
    config => {
        console.log('📤 Petición:', config.method.toUpperCase(), config.url);
        console.log('📤 Datos:', config.data);
        return config;
    },
    error => {
        console.error('📤 Error en petición:', error);
        return Promise.reject(error);
    }
);

// Interceptor para ver todas las respuestas
api.interceptors.response.use(
    response => {
        console.log('📥 Respuesta:', response.status, response.config.url);
        console.log('📥 Datos:', response.data);
        return response;
    },
    error => {
        if (error.code === 'ECONNABORTED') {
            console.error('📥 Timeout - El servidor no responde');
        } else if (!error.response) {
            console.error('📥 Error de red - ¿El backend está corriendo?');
            console.error('📥 URL intentada:', error.config?.baseURL + error.config?.url);
        } else {
            console.error('📥 Error:', error.response.status, error.response.data);
        }
        return Promise.reject(error);
    }
);

// Prueba de conexión al iniciar
const testConnection = async () => {
    try {
        console.log('🔄 Probando conexión con el backend...');
        await api.get('/Citas');
        console.log('✅ Conexión exitosa con el backend');
    } catch (error) {
        console.error('❌ No se puede conectar con el backend');
        console.error('❌ Verifica que el backend esté corriendo en:', API_URL);
    }
};

// Ejecutar prueba de conexión
testConnection();

export const citasService = {
    getAll: () => api.get('/Citas'),
    getById: (id) => api.get(`/Citas/${id}`),
    create: (data) => api.post('/Citas', data),
    update: (id, data) => api.put(`/Citas/${id}`, { ...data, id }),
    delete: (id) => api.delete(`/Citas/${id}`),
};

export const reservasService = {
    getAll: () => api.get('/Reservas'),
    getById: (id) => api.get(`/Reservas/${id}`),
    create: (data) => api.post('/Reservas', data),
    update: (id, data) => api.put(`/Reservas/${id}`, { ...data, id }),
    delete: (id) => api.delete(`/Reservas/${id}`),
};