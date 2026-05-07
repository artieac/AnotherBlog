import React from 'react';
import ReactQuill from 'react-quill';
import 'react-quill/dist/quill.snow.css';

interface WysiwygEditorProps {
    value: string;
    onChange: (content: string) => void;
    label?: string;
}

export const WysiwygEditor: React.FC<WysiwygEditorProps> = ({ value, onChange, label }) => {
    const modules = {
        toolbar: [
            [{ 'header': [1, 2, false] }],
            ['bold', 'italic', 'underline', 'strike', 'blockquote'],
            [{ 'list': 'ordered' }, { 'list': 'bullet' }, { 'indent': '-1' }, { 'indent': '+1' }],
            ['link', 'image'],
            ['clean']
        ],
    };

    const formats = [
        'header',
        'bold', 'italic', 'underline', 'strike', 'blockquote',
        'list', 'bullet', 'indent',
        'link', 'image'
    ];

    return (
        <div className="mb-4">
            {label && <label className="block text-sm font-medium text-gray-700 mb-1">{label}</label>}
            <div className="bg-white">
                <ReactQuill 
                    theme="snow"
                    value={value || ''}
                    onChange={onChange}
                    modules={modules}
                    formats={formats}
                />
            </div>
        </div>
    );
};
