import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { fetchSiteInfo, saveSiteInfo } from '@/redux/SiteSlice';
import { RootState, AppDispatch } from '@/redux/store';
import { TextInput } from '@/components/TextInput';
import { Button } from '@/components/Button';
import { WysiwygEditor } from '@/components/WysiwygEditor';
import { ISiteInfo } from '@/Models/ISiteInfo';

export const SiteInfoPage: React.FC = () => {
    const dispatch = useDispatch<AppDispatch>();
    const { siteInfo, loading, error } = useSelector((state: RootState) => state.site);
    const [formData, setFormData] = useState<ISiteInfo | null>(null);

    useEffect(() => {
        dispatch(fetchSiteInfo());
    }, [dispatch]);

    useEffect(() => {
        if (siteInfo) {
            setFormData(siteInfo);
        }
    }, [siteInfo]);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        if (formData) {
            const { name, value } = e.target;
            const propertyName = name.charAt(0).toUpperCase() + name.slice(1);
            setFormData({ ...formData, [propertyName as keyof ISiteInfo]: value } as ISiteInfo);
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
            await dispatch(saveSiteInfo(formData));
            alert('Site Info saved successfully!');
        }
    };

    if (loading || !formData) return <div>Loading...</div>;
    if (error) return <div className="text-red-600">Error: {error}</div>;

    return (
        <div className="bg-white shadow rounded-lg p-6">
            <h1 className="text-2xl font-bold text-gray-900 mb-6">Edit Site Info</h1>
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
                    label="Contact Email"
                    id="contactEmail"
                    name="contactEmail"
                    value={formData.ContactEmail}
                    onChange={handleChange}
                />
                <TextInput
                    label="Site Analytics ID"
                    id="siteAnalyticsId"
                    name="siteAnalyticsId"
                    value={formData.SiteAnalyticsId}
                    onChange={handleChange}
                />
                <TextInput
                    label="Default Author"
                    id="defaultAuthor"
                    name="defaultAuthor"
                    value={formData.DefaultAuthor}
                    onChange={handleChange}
                />
                <TextInput
                    label="Default Keywords"
                    id="defaultKeywords"
                    name="defaultKeywords"
                    value={formData.DefaultKeywords}
                    onChange={handleChange}
                />
                <WysiwygEditor
                    label="About"
                    value={formData.About}
                    onBlur={handleAboutChange}
                />
                <div className="flex space-x-4">
                    <Button type="submit" variant="primary">Save</Button>
                </div>
            </form>
        </div>
    );
};
