import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { fetchBlogById, saveBlog, setCurrentBlog } from '@/redux/BlogSlice';
import { RootState, AppDispatch } from '@/redux/store';
import { TextInput } from '@/components/TextInput';
import { Button } from '@/components/Button';
import { WysiwygEditor } from '@/components/WysiwygEditor';
import { IBlog } from '@/Models/IBlog';

export const EditBlogPage: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const dispatch = useDispatch<AppDispatch>();
    const { currentBlog, loading, error } = useSelector((state: RootState) => state.blogs);
    const [formData, setFormData] = useState<IBlog | null>(null);

    useEffect(() => {
        if (id && id !== '-1') {
            dispatch(fetchBlogById(parseInt(id)));
        } else {
            dispatch(setCurrentBlog({
                Id: -1,
                Name: '',
                Description: '',
                SubFolder: '',
                About: '',
                WelcomeMessage: '',
                Theme: 'default'
            }));
        }
    }, [id, dispatch]);

    useEffect(() => {
        if (currentBlog) {
            setFormData(currentBlog);
        }
    }, [currentBlog]);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        if (formData) {
            const { name, value } = e.target;
            // Map input names to PascalCase if they aren't already
            const propertyName = name.charAt(0).toUpperCase() + name.slice(1);
            setFormData({ ...formData, [propertyName as keyof IBlog]: value } as IBlog);
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
            await dispatch(saveBlog(formData));
            navigate('/Admin/App/ManageBlogs');
        }
    };

    if (loading || !formData) return <div>Loading...</div>;
    if (error) return <div className="text-red-600">Error: {error}</div>;

    return (
        <div className="bg-white shadow rounded-lg p-6">
            <h1 className="text-2xl font-bold text-gray-900 mb-6">{formData.Id === -1 ? 'Add' : 'Edit'} Blog</h1>
            <form onSubmit={handleSubmit}>
                <TextInput
                    label="Name"
                    id="name"
                    name="name"
                    value={formData.Name}
                    onChange={handleChange}
                    required
                />
                <TextInput
                    label="Description"
                    id="description"
                    name="description"
                    value={formData.Description}
                    onChange={handleChange}
                />
                <TextInput
                    label="SubFolder"
                    id="subFolder"
                    name="subFolder"
                    value={formData.SubFolder}
                    onChange={handleChange}
                    required
                />
                <TextInput
                    label="Welcome Message"
                    id="welcomeMessage"
                    name="welcomeMessage"
                    value={formData.WelcomeMessage}
                    onChange={handleChange}
                />
                <WysiwygEditor
                    key={formData.Id}
                    label="About"
                    value={formData.About}
                    onChange={handleAboutChange}
                    showPreview={true}
                />
                <div className="flex space-x-4">
                    <Button type="submit" variant="primary">Save</Button>
                    <Button type="button" variant="secondary" onClick={() => navigate('/Admin/App/ManageBlogs')}>Cancel</Button>
                </div>
            </form>
        </div>
    );
};
