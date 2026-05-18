import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { fetchUserById, saveUser } from '@/redux/UserSlice';
import { fetchBlogs } from '@/redux/BlogSlice';
import { RootState, AppDispatch } from '@/redux/store';
import { TextInput } from '@/components/TextInput';
import { Button } from '@/components/Button';
import { WysiwygEditor } from '@/components/WysiwygEditor';
import { Table } from '@/components/Table';
import { IUser } from '@/types/user.types';
import { IBlog } from '@/types/blog.types';

const ROLE_OPTIONS = [
    { label: 'None', value: 0 },
    { label: 'Administrator', value: 1 },
    { label: 'Blogger', value: 2 },
    { label: 'Reader', value: 3 },
];

export const EditUserPage: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const dispatch = useDispatch<AppDispatch>();
    const { selectedUser, loading: userLoading, error: userError } = useSelector((state: RootState) => state.users);
    const { blogs, loading: blogsLoading } = useSelector((state: RootState) => state.blogs);
    const [formData, setFormData] = useState<IUser | null>(null);

    useEffect(() => {
        dispatch(fetchBlogs());
        if (id) {
            dispatch(fetchUserById(parseInt(id)));
        }
    }, [id, dispatch]);

    useEffect(() => {
        if (selectedUser) {
            setFormData({
                ...selectedUser,
                Roles: selectedUser.Roles || {}
            });
        }
    }, [selectedUser]);

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

    const handleRoleChange = (blogId: number, roleId: number) => {
        if (formData) {
            const newRoles = { ...formData.Roles };
            if (roleId === 0) {
                delete newRoles[blogId];
            } else {
                newRoles[blogId] = roleId;
            }
            setFormData({ ...formData, Roles: newRoles });
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

    if (userLoading || blogsLoading || !formData) return <div>Loading...</div>;
    if (userError) return <div className="text-red-600">Error: {userError}</div>;

    const blogColumns = [
        { header: 'Blog Name', key: 'Name' },
        { 
            header: 'Role', 
            key: 'Role', 
            render: (blog: IBlog) => (
                <select
                    value={formData.Roles[blog.Id] || 0}
                    onChange={(e) => handleRoleChange(blog.Id, parseInt(e.target.value))}
                    className="block w-full pl-3 pr-10 py-2 text-base border-gray-300 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm rounded-md"
                >
                    {ROLE_OPTIONS.map(option => (
                        <option key={option.value} value={option.value}>{option.label}</option>
                    ))}
                </select>
            )
        },
    ];

    return (
        <div className="bg-white shadow rounded-lg p-6">
            <h1 className="text-2xl font-bold text-gray-900 mb-6">Edit User</h1>
            <form onSubmit={handleSubmit}>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-6 mb-6">
                    <div>
                        <TextInput
                            label="First Name"
                            id="firstName"
                            name="firstName"
                            value={formData.FirstName}
                            onChange={handleChange}
                        />
                        <TextInput
                            label="Last Name"
                            id="lastName"
                            name="lastName"
                            value={formData.LastName}
                            onChange={handleChange}
                        />
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
                    </div>
                    <div>
                        <h2 className="text-lg font-medium text-gray-900 mb-4">Blog Roles</h2>
                        <Table
                            keyField="Id"
                            columns={blogColumns}
                            data={blogs}
                        />
                    </div>
                </div>

                <WysiwygEditor
                    key={formData.Id}
                    label="About"
                    value={formData.About}
                    onChange={handleAboutChange}
                    showPreview={true}
                />
                <div className="flex space-x-4 mt-6">
                    <Button type="submit" variant="primary">Save</Button>
                    <Button type="button" variant="secondary" onClick={() => navigate('/Admin/App/ManageUsers')}>Cancel</Button>
                </div>
            </form>
        </div>
    );
};

