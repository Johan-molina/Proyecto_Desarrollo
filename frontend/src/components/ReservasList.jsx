import React, { useState, useEffect } from 'react';
import { Table, Button, Container, Row, Col, Card, Alert, Badge, Spinner } from 'react-bootstrap';
import { Pencil, Trash, Plus, HouseDoor } from 'react-bootstrap-icons';
import { reservasService } from '../services/api';
import ReservasForm from './ReservasForm';

const ReservasList = () => {
    const [reservas, setReservas] = useState([]);
    const [loading, setLoading] = useState(true);
    const [showForm, setShowForm] = useState(false);
    const [selectedReserva, setSelectedReserva] = useState(null);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    // Cargar reservas al montar el componente
    useEffect(() => {
        loadReservas();
    }, []);

    const loadReservas = async () => {
        setLoading(true);
        setError('');
        try {
            console.log('Cargando reservas...');
            const response = await reservasService.getAll();
            console.log('Reservas recibidas:', response.data);
            setReservas(response.data || []);
        } catch (error) {
            console.error('Error detallado al cargar reservas:', error);
            console.error('Respuesta del servidor:', error.response?.data);
            setError('Error al cargar las reservas: ' + (error.response?.data?.title || error.message));
        } finally {
            setLoading(false);
        }
    };

    const handleDelete = async (id) => {
        if (window.confirm('¿Estás seguro de eliminar esta reserva?')) {
            try {
                await reservasService.delete(id);
                setSuccess('Reserva eliminada correctamente');
                loadReservas(); // Recargar la lista
                setTimeout(() => setSuccess(''), 3000);
            } catch (error) {
                console.error('Error al eliminar:', error);
                setError('Error al eliminar la reserva');
            }
        }
    };

    const handleEdit = (reserva) => {
        setSelectedReserva(reserva);
        setShowForm(true);
    };

    const handleAdd = () => {
        setSelectedReserva(null);
        setShowForm(true);
    };

    const handleCloseForm = () => {
        setShowForm(false);
        setSelectedReserva(null);
    };

    const handleSave = () => {
        loadReservas();
        setSuccess(`Reserva ${selectedReserva ? 'actualizada' : 'creada'} correctamente`);
        setTimeout(() => setSuccess(''), 3000);
    };

    const formatDate = (dateString) => {
        if (!dateString) return '-';
        try {
            return new Date(dateString).toLocaleString('es-ES', {
                year: 'numeric',
                month: '2-digit',
                day: '2-digit',
                hour: '2-digit',
                minute: '2-digit'
            });
        } catch (error) {
            return dateString;
        }
    };

    if (loading) {
        return (
            <Container className="text-center mt-5">
                <Spinner animation="border" variant="primary" />
                <p className="mt-2 text-white">Cargando reservas...</p>
            </Container>
        );
    }

    return (
        <Container>
            <Row className="mb-4 align-items-center">
                <Col>
                    <h2 className="text-white">
                        <HouseDoor className="me-2" />
                        Gestión de Reservas
                    </h2>
                </Col>
                <Col xs="auto">
                    <Button variant="success" onClick={handleAdd}>
                        <Plus /> Nueva Reserva
                    </Button>
                </Col>
            </Row>

            {error && (
                <Alert variant="danger" onClose={() => setError('')} dismissible>
                    <Alert.Heading>Error</Alert.Heading>
                    <p>{error}</p>
                </Alert>
            )}

            {success && (
                <Alert variant="success" onClose={() => setSuccess('')} dismissible>
                    {success}
                </Alert>
            )}

            <Card>
                <Card.Body>
                    <Table striped hover responsive>
                        <thead>
                            <tr>
                                <th>ID</th>
                                <th>Cliente</th>
                                <th>Entrada</th>
                                <th>Salida</th>
                                <th>Email</th>
                                <th>Teléfono</th>
                                <th>Acciones</th>
                            </tr>
                        </thead>
                        <tbody>
                            {reservas.length === 0 ? (
                                <tr>
                                    <td colSpan="7" className="text-center py-4">
                                        <p className="mb-0">No hay reservas registradas</p>
                                        <Button
                                            variant="link"
                                            onClick={handleAdd}
                                            className="mt-2"
                                        >
                                            Crear primera reserva
                                        </Button>
                                    </td>
                                </tr>
                            ) : (
                                reservas.map((reserva) => (
                                    <tr key={reserva.id}>
                                        <td>
                                            <Badge bg="info">{reserva.id}</Badge>
                                        </td>
                                        <td>{reserva.nombreCliente}</td>
                                        <td>{formatDate(reserva.fechaEntrada)}</td>
                                        <td>{formatDate(reserva.fechaSalida)}</td>
                                        <td>{reserva.email || '-'}</td>
                                        <td>{reserva.telefono || '-'}</td>
                                        <td>
                                            <Button
                                                variant="warning"
                                                size="sm"
                                                onClick={() => handleEdit(reserva)}
                                                className="me-2"
                                                title="Editar reserva"
                                            >
                                                <Pencil /> Editar
                                            </Button>
                                            <Button
                                                variant="danger"
                                                size="sm"
                                                onClick={() => handleDelete(reserva.id)}
                                                title="Eliminar reserva"
                                            >
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

            <ReservasForm
                show={showForm}
                handleClose={handleCloseForm}
                reserva={selectedReserva}
                onSave={handleSave}
            />
        </Container>
    );
};

export default ReservasList;