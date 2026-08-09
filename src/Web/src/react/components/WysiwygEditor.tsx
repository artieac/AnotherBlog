import React, { useState } from 'react';
import ReactQuill, { Quill } from 'react-quill-new';
import 'react-quill-new/dist/quill.snow.css';
import QuillTableBetter from 'quill-table-better';
import 'quill-table-better/dist/quill-table-better.css';

Quill.register({ 'modules/table-better': QuillTableBetter }, true);

interface WysiwygEditorProps {
    value: string;
    onChange: (content: string) => void;
    label?: string;
    showPreview?: boolean;
}

export const WysiwygEditor: React.FC<WysiwygEditorProps> = ({ value, onChange, label, showPreview = false }) => {
    const [activeTab, setActiveTab] = useState<'edit' | 'preview'>('edit');

    const modules = {
        table: false,
        toolbar: [
            [{ 'header': [1, 2, 3, false] }],
            ['bold', 'italic', 'underline', 'strike', 'blockquote'],
            [{ 'list': 'ordered' }, { 'list': 'bullet' }, { 'indent': '-1' }, { 'indent': '+1' }],
            ['link', 'image'],
            ['table-better'],
            ['clean']
        ],
        'table-better': {
            language: 'en_US',
            menus: ['column', 'row', 'merge', 'table', 'cell', 'wrap', 'copy', 'delete'],
            toolbarTable: true
        },
        keyboard: {
            bindings: QuillTableBetter.keyboardBindings
        }
    };

    return (
        <div className="mb-4">
            <div className="flex justify-between items-center mb-1">
                {label && <label className="block text-sm font-medium text-gray-700">{label}</label>}
                {showPreview && (
                    <div className="flex border-b border-gray-200">
                        <button
                            type="button"
                            className={`py-1 px-4 text-sm font-medium ${activeTab === 'edit' ? 'text-blue-600 border-b-2 border-blue-600' : 'text-gray-500 hover:text-gray-700'}`}
                            onClick={() => setActiveTab('edit')}
                        >
                            Edit
                        </button>
                        <button
                            type="button"
                            className={`py-1 px-4 text-sm font-medium ${activeTab === 'preview' ? 'text-blue-600 border-b-2 border-blue-600' : 'text-gray-500 hover:text-gray-700'}`}
                            onClick={() => setActiveTab('preview')}
                        >
                            Preview
                        </button>
                    </div>
                )}
            </div>
            <div className="bg-white border rounded-md overflow-hidden">
                {activeTab === 'edit' ? (
                    <ReactQuill 
                        theme="snow"
                        value={value || ''}
                        onChange={onChange}
                        modules={modules}
                    />
                ) : (
                    <div className="ql-container ql-snow" style={{ border: 'none' }}>
                        <div 
                            className="ql-editor p-4 min-h-[200px]"
                            dangerouslySetInnerHTML={{ __html: value || '' }}
                        />
                    </div>
                )}
            </div>
        </div>
    );
};
