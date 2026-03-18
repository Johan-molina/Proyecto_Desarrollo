import React, { useState, useEffect } from 'react';
import { Table, Button, Container, Row, Col, Card, Alert, Badge } from 'react-bootstrap';
import { Pencil, Trash, Plus, Calendar } from 'react-bootstrap-icons';
import { citasService } from '../services/api';
import CitasForm from './CitasForm';

const CitasList = () => {
    const [citas, setCitas] = useState([]);
    const [showForm, setShowForm] = useState(false);
    const [selectedCita, setSelectedCita] = useState(null);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    useEffect(() => {
        loadCitas();
    }, []);

    const loadCitas = async () => {
        try {
            const response = await citasService.getAll();
            setCitas(response.data);
        } catch (error) {
            setError('Error al cargar las citas');
        }
    };

    const handleDelete = async (id) => {
        if (window.confirm('¿Estás seguro de eliminar esta cita?')) {
            try {
                await citasService.delete(id);
                setSuccess('Cita eliminada correctamente');
                loadCitas();
                setTimeout(() => setSuccess(''), 3000);
            } catch (error) {
                setError('Error al eliminar la cita');
            }
        }
    };

    const handleEdit = (cita) => {
        setSelectedCita(cita);
        setShowForm(true);
    };

    const handleAdd = () => {
        setSelectedCita(null);
        setShowForm(true);
    };

    const handleCloseForm = () => {
        setShowForm(false);
        setSelectedCita(null);
    };

    const formatDate = (dateString) => {
        return new Date(dateString).toLocaleString('es-ES', {
            year: 'numeric',
            month: '2-digit',
            day: '2-digit',
            hour: '2-digit',
            minute: '2-digit'
        });
    };

    return (
        <Container>
            <Row className="mb-4">
                <Col>
                    <h2 className="text-white">
                        <Calendar className="me-2" />
                        Gestión de Citas
                    </h2>
                </Col>
                <Col xs="auto">
                    <Button variant="success" onClick={handleAdd}>
                        <Plus /> Nueva Cita
                    </Button>
                </Col>
            </Row>

            {error && <Alert variant="danger" onClose={() => setError('')} dismissible>{error}</Alert>}
            {success && <Alert variant="success" onClose={() => setSuccess('')} dismissible>{success}</Alert>}

            <Card>
                <Card.Body>
                    <Table striped hover responsive>
                        <thead>
                            <tr>
                                <th>ID</th>
                                <th>Cliente</th>
                                <th>Fecha</th>
                                <th>Motivo</th>
                                <th>Email</th>
                                <th>Teléfono</th>
                                <th>Acciones</th>
                            </tr>
                        </thead>
                        <tbody>
                            {citas.length === 0 ? (
                                <tr>
                                    <td colSpan="7" className="text-center">No hay citas registradas</td>
                                </tr>
                            ) : (
                                citas.map((cita) => (
                                    <tr key={cita.id}>
                                        <td><Badge bg="info">{cita.id}</Badge></td>
                                        <td>{cita.nombreCliente}</td>
                                        <td>{formatDate(cita.fecha)}</td>
                                        <td>{cita.motivo || '-'}</td>
                                        <td>{cita.email || '-'}</td>
                                        <td>{cita.telefono || '-'}</td>
                                        <td>
                                            <Button variant="warning" size="sm" onClick={() => handleEdit(cita)} className="me-2">
                                                <Pencil /> Editar
                                            </Button>
                                            <Button variant="danger" size="sm" onClick={() => handleDelete(cita.id)}>
                                                <Trash /> Eliminar
                                            </Button>
                                        </td>
                                    </tr>
                                ))
                            )}
                        </tbody>
                    </Table>
                </Card.Body>
            </Card>

            <CitasForm
                show={showForm}
                handleClose={handleCloseForm}
                cita={selectedCita}
                onSave={() => {
                    loadCitas();
                    setSuccess(`Cita ${selectedCita ? 'actualizada' : 'creada'} correctamente`);
                    setTimeout(() => setSuccess(''), 3000);
                }}
            />
        </Container>
    );
};

export default CitasList;