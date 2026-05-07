import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { fetchPostById, savePost, setCurrentPost } from '@/redux/BlogPostSlice';
import { RootState, AppDispatch } from '@/redux/store';
import { TextInput } from '@/components/TextInput';
import { Button } from '@/components/Button';
import { WysiwygEditor } from '@/components/WysiwygEditor';
import { IBlogPost } from '@/Models/IBlogPost';

export const EditPostPage: React.FC = () => {
    const { blogSubFolder, id } = useParams<{ blogSubFolder: string, id: string }>();
    const navigate = useNavigate();
    const dispatch = useDispatch<AppDispatch>();
    const { currentPost, loading, error } = useSelector((state: RootState) => state.blogPosts);
    const [formData, setFormData] = useState<IBlogPost | null>(null);

    useEffect(() => {
        if (id && id !== '0' && id !== '-1' && blogSubFolder) {
            dispatch(fetchPostById({ blogSubFolder, id: parseInt(id) }));
        } else {
            dispatch(setCurrentPost({
                Id: 0,
                Title: '',
                EntryText: '',
                IsPublished: false,
                DatePosted: new Date().toISOString(),
                DateCreated: new Date().toISOString(),
                CommentCount: 0,
                TimesViewed: 0,
                Tags: []
            } as any));
        }
    }, [id, blogSubFolder, dispatch]);

    useEffect(() => {
        if (currentPost) {
            setFormData(currentPost);
        }
    }, [currentPost]);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        if (formData) {
            const { name, value, type } = e.target;
            const checked = (e.target as HTMLInputElement).checked;
            const propertyName = name.charAt(0).toUpperCase() + name.slice(1);
            
            setFormData({ 
                ...formData, 
                [propertyName as keyof IBlogPost]: type === 'checkbox' ? checked : value 
            } as IBlogPost);
        }
    };

    const handleTextChange = (content: string) => {
        if (formData) {
            setFormData({ ...formData, EntryText: content });
        }
    };

    const handleTagsChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (formData) {
            const tagNames = e.target.value.split(',');
            setFormData({
                ...formData,
                Tags: tagNames.map(name => ({ Name: name.trim() } as any))
            });
        }
    }

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (formData && blogSubFolder) {
            await dispatch(savePost({ blogSubFolder, post: formData }));
            navigate(`/Admin/App/ManagePosts/${blogSubFolder}`);
        }
    };

    if (loading || !formData) return <div>Loading...</div>;
    if (error) return <div className="text-red-600">Error: {error}</div>;

    const tagsString = formData.Tags.map(t => t.Name).join(', ');

    return (
        <div className="bg-white shadow rounded-lg p-6">
            <h1 className="text-2xl font-bold text-gray-900 mb-6">{formData.Id <= 0 ? 'Add' : 'Edit'} Post</h1>
            <form onSubmit={handleSubmit}>
                <TextInput
                    label="Title"
                    id="title"
                    name="title"
                    value={formData.Title}
                    onChange={handleChange}
                    required
                />
                <div className="mb-4">
                    <label className="flex items-center">
                        <input
                            type="checkbox"
                            name="isPublished"
                            checked={formData.IsPublished}
                            onChange={handleChange}
                            className="h-4 w-4 text-blue-600 focus:ring-blue-500 border-gray-300 rounded"
                        />
                        <span className="ml-2 text-sm text-gray-700">Published</span>
                    </label>
                </div>
                <TextInput
                    label="Tags (comma separated)"
                    id="tags"
                    name="tags"
                    value={tagsString}
                    onChange={handleTagsChange}
                />
                <WysiwygEditor
                    label="Content"
                    value={formData.EntryText}
                    onBlur={handleTextChange}
                />
                <div className="flex space-x-4">
                    <Button type="submit" variant="primary">Save</Button>
                    <Button type="button" variant="secondary" onClick={() => navigate(`/Admin/App/ManagePosts/${blogSubFolder}`)}>Cancel</Button>
                </div>
            </form>
        </div>
    );
};
