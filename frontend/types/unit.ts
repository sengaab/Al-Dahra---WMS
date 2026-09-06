export interface UnitDto {
    unitId: number;

    name: string;
    abbreviation: string;

    createdAt: string;
    updatedAt: string;
}

export interface CreateUnitDto {
    name: string;
    abbreviation: string;
}

export interface UpdateUnitDto {
    name: string;
    abbreviation: string;
}

export interface DeleteUnitResponseDto {
    message: string;
}