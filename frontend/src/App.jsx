import React from 'react';
import { BrowserRouter, Routes, Route, Navigate, Link } from 'react-router-dom';
import { Container } from 'react-bootstrap';
import NavigationBar from './components/Navbar';
import CitasList from './components/CitasList';
import ReservasList from './components/ReservasList';
import './App.css';

function Home() {
    return (
        <div className="home">
            <h1 className="title">Sistema de Gestión Hotelera</h1>
            <p className="subtitle">Administra citas y reservas fácilmente</p>

            <div className="cards-container">
                <div className="custom-card">
                    <h2>📅 Citas</h2>
                    <p>Gestiona las citas de tus clientes</p>
                    <Link to="/citas" className="btn-custom">Ir a Citas</Link>
                </div>

                <div className="custom-card">
                    <h2>🏠 Reservas</h2>
                    <p>Administra las reservas del hotel</p>
                    <Link to="/reservas" className="btn-custom">Ir a Reservas</Link>
                </div>
            </div>
        </div>
    );
}

function App() {
    return (
        <BrowserRouter>
            <NavigationBar />
            <Container className="py-4">
                <Routes>
                    <Route path="/" element={<Home />} />
                    <Route path="/citas" element={<CitasList />} />
                    <Route path="/reservas" element={<ReservasList />} />
                    <Route path="*" element={<Navigate to="/" />} />
                </Routes>
            </Container>
        </BrowserRouter>
    );
}

export default App;