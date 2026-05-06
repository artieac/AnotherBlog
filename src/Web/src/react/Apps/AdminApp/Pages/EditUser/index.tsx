import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { fetchUserById, saveUser } from '@/redux/UserSlice';
import { RootState, AppDispatch } from '@/redux/store';
import { TextInput } from '@/components/TextInput';
import { Button } from '@/components/Button';
import { WysiwygEditor } from '@/components/WysiwygEditor';
import { IUser } from '@/Models/IUser';

export const EditUserPage: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const dispatch = useDispatch<AppDispatch>();
    const { currentUser, loading, error } = useSelector((state: RootState) => state.users);
    const [formData, setFormData] = useState<IUser | null>(null);

    useEffect(() => {
        if (id) {
            dispatch(fetchUserById(parseInt(id)));
        }
    }, [id, dispatch]);

    useEffect(() => {
        if (currentUser) {
            setFormData(currentUser);
        }
    }, [currentUser]);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        if (formData) {
            const { name, value, type } = e.target;
            const checked = (e.target as HTMLInputElement).checked;
            const propertyName = name.charAt(0).toUpperCase() + name.slice(1);
            
            setFormData({ 
                ...formData, 
                [propertyName as keyof IUser]: type === 'checkbox' ? checked : value 
            } as IUser);
        }
    };

    const handleAboutChange = (content: string) => {
        if (formData) {
            setFormData({ ...formData, About: content });
        }
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (formData) {
            await dispatch(saveUser(formData));
            navigate('/Admin/App/ManageUsers');
        }
    };

    if (loading || !formData) return <div>Loading...</div>;
    if (error) return <div className="text-red-600">Error: {error}</div>;

    return (
        <div className="bg-white shadow rounded-lg p-6">
            <h1 className="text-2xl font-bold text-gray-900 mb-6">Edit User</h1>
            <form onSubmit={handleSubmit}>
                <TextInput
                    label="Display Name"
                    id="displayName"
                    name="displayName"
                    value={formData.DisplayName}
                    onChange={handleChange}
                    required
                />
                <div className="mb-4">
                    <label className="flex items-center">
                        <input
                            type="checkbox"
                            name="isSiteAdministrator"
                            checked={formData.IsSiteAdministrator}
                            onChange={handleChange}
                            className="h-4 w-4 text-blue-600 focus:ring-blue-500 border-gray-300 rounded"
                        />
                        <span className="ml-2 text-sm text-gray-700">Site Administrator</span>
                    </label>
                </div>
                <div className="mb-4">
                    <label className="flex items-center">
                        <input
                            type="checkbox"
                            name="approvedCommenter"
                            checked={formData.ApprovedCommenter}
                            onChange={handleChange}
                            className="h-4 w-4 text-blue-600 focus:ring-blue-500 border-gray-300 rounded"
                        />
                        <span className="ml-2 text-sm text-gray-700">Approved Commenter</span>
                    </label>
                </div>
                <WysiwygEditor
                    label="About"
                    value={formData.About}
                    onBlur={handleAboutChange}
                />
                <div className="flex space-x-4">
                    <Button type="submit" variant="primary">Save</Button>
                    <Button type="button" variant="secondary" onClick={() => navigate('/Admin/App/ManageUsers')}>Cancel</Button>
                </div>
            </form>
        </div>
    );
};
