import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Link, useNavigate } from 'react-router-dom';
import { fetchUsers, deleteUser } from '@/redux/UserSlice';
import { RootState, AppDispatch } from '@/redux/store';
import { Table } from '@/components/Table';
import { Button } from '@/components/Button';
import { IUser } from '@/Models/IUser';

export const ManageUsersPage: React.FC = () => {
    const dispatch = useDispatch<AppDispatch>();
    const navigate = useNavigate();
    const { users, loading, error } = useSelector((state: RootState) => state.users);

    useEffect(() => {
        dispatch(fetchUsers());
    }, [dispatch]);

    const handleDelete = async (id: number) => {
        if (window.confirm('Are you sure you want to delete this user?')) {
            await dispatch(deleteUser(id));
        }
    };

    const columns = [
        { header: 'Display Name', key: 'DisplayName', render: (user: IUser) => <Link to={`/Admin/App/EditUser/${user.Id}`} className="text-blue-600 hover:underline">{user.DisplayName}</Link> },
        { header: 'User Name', key: 'UserName' },
        { header: 'Email', key: 'Email' },
        { 
            header: 'Actions', 
            key: 'Id', 
            render: (user: IUser) => (
                <div className="flex space-x-2">
                    <Button variant="secondary" onClick={() => navigate(`/Admin/App/EditUser/${user.Id}`)}>Edit</Button>
                    <Button variant="danger" onClick={() => handleDelete(user.Id)}>Delete</Button>
                </div>
            )
        },
    ];

    if (loading) return <div>Loading...</div>;
    if (error) return <div className="text-red-600">Error: {error}</div>;

    return (
        <div className="bg-white shadow rounded-lg p-6">
            <div className="flex justify-between items-center mb-6">
                <h1 className="text-2xl font-bold text-gray-900">Manage Users</h1>
            </div>
            <Table data={users} columns={columns} keyField="Id" />
        </div>
    );
};
